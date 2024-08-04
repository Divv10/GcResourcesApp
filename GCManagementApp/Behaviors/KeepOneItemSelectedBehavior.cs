using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GCManagementApp.Behaviors
{
    public sealed class KeepOneItemSelectedBehavior : Behavior<ListBox>
    {
        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>
        /// Override this to hook up functionality to the AssociatedObject.
        /// </remarks>
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectionChanged += AssociatedObjectOnSelectionChanged;
        }

        /// <summary>
        /// Selection changed event handler that prevents the last selected item in the listbox from being deselected.
        /// Only tested with SingleSelection list boxes, should work with others.
        /// </summary>
        /// <param name="sender">ListBox that raised the event.</param>
        /// <param name="e">Selection changed information.</param>
        private void AssociatedObjectOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listbox = ((ListBox)sender);
            if (listbox.SelectedItem == null)
            {
                if (e.RemovedItems.Count > 0)
                {
                    object itemToReselect = e.RemovedItems[0];
                    if (listbox.Items.Contains(itemToReselect)) // only reselect if the item is still part of the list.
                    {
                        listbox.SelectedItem = itemToReselect;
                    }
                }
            }
        }

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        /// <remarks>
        /// Override this to unhook functionality from the AssociatedObject.
        /// </remarks>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.SelectionChanged -= AssociatedObjectOnSelectionChanged;
        }
    }
}
