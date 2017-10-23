using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Controls;

namespace Cozyplanes.SudokuApp
{
    public class DataGridRollbackOnUnfocusedBehaviour
    {

        public static bool GetDataGridRollbackOnUnfocused(DataGrid DataGridSudoku)
        {
            return (bool)DataGridSudoku.GetValue(DataGridRollbackOnUnfocusedProperty);
        }

        public static void SetDataGridRollbackOnUnfocused(
          DataGrid DataGridSudoku, bool value)
        {
            DataGridSudoku.SetValue(DataGridRollbackOnUnfocusedProperty, value);
        }

        public static readonly DependencyProperty DataGridRollbackOnUnfocusedProperty =
            DependencyProperty.RegisterAttached(
            "DataGridRollbackOnUnfocused",
            typeof(bool),
            typeof(DataGridRollbackOnUnfocusedBehaviour),
            new UIPropertyMetadata(false, OnDataGridRollbackOnUnfocusedChanged));

        static void OnDataGridRollbackOnUnfocusedChanged(
          DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            DataGrid DataGridSudoku = depObj as DataGrid;
            if (DataGridSudoku == null)
                return;

            if (e.NewValue is bool == false)
                return;

            if ((bool)e.NewValue)
            {
                DataGridSudoku.LostKeyboardFocus += RollbackDataGridOnLostFocus;
                DataGridSudoku.DataContextChanged += RollbackDataGridOnDataContextChanged;
            }
            else
            {
                DataGridSudoku.LostKeyboardFocus -= RollbackDataGridOnLostFocus;
                DataGridSudoku.DataContextChanged -= RollbackDataGridOnDataContextChanged;
            }
        }

        static void RollbackDataGridOnLostFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            DataGrid senderDatagrid = sender as DataGrid;

            if (senderDatagrid == null)
                return;

            UIElement focusedElement = Keyboard.FocusedElement as UIElement;
            if (focusedElement == null)
                return;

            DataGrid focusedDatagrid = GetParentDatagrid(focusedElement); //let's see if the new focused element is inside a datagrid
            if (focusedDatagrid == senderDatagrid)
            {
                return;
                //if the new focused element is inside the same datagrid, then we don't need to do anything;
                //this happens, for instance, when we enter in edit-mode: the DataGrid element loses keyboard-focus, which passes to the selected DataGridCell child
            }

            //otherwise, the focus went outside the datagrid; in order to avoid exceptions like ("DeferRefresh' is not allowed during an AddNew or EditItem transaction")
            //or ("CommitNew is not allowed for this view"), we undo the possible pending changes, if any
            IEditableCollectionView collection = senderDatagrid.Items as IEditableCollectionView;

            if (collection.IsEditingItem)
            {
                collection.CommitEdit();
            }
            else if (collection.IsAddingNew)
            {
                collection.CancelNew();
            }
        }

        private static DataGrid GetParentDatagrid(UIElement element)
        {
            UIElement childElement; //element from which to start the tree navigation, looking for a Datagrid parent

            if (element is ComboBoxItem) //since ComboBoxItem.Parent is null, we must pass through ItemsPresenter in order to get the parent ComboBox
            {
                ItemsPresenter parentItemsPresenter = VisualTreeFinder.FindParentControl<ItemsPresenter>((element as ComboBoxItem));
                ComboBox combobox = parentItemsPresenter.TemplatedParent as ComboBox;
                childElement = combobox;
            }
            else
            {
                childElement = element;
            }

            DataGrid parentDatagrid = VisualTreeFinder.FindParentControl<DataGrid>(childElement); //let's see if the new focused element is inside a datagrid
            return parentDatagrid;
        }

        static void RollbackDataGridOnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            DataGrid senderDatagrid = sender as DataGrid;

            if (senderDatagrid == null)
                return;

            IEditableCollectionView collection = senderDatagrid.Items as IEditableCollectionView;

            if (collection.IsEditingItem)
            {
                collection.CancelEdit();
            }
            else if (collection.IsAddingNew)
            {
                collection.CancelNew();
            }
        }
        
    }
}
