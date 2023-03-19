using Koh.Wpf.Controls.ObjectEditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Koh.Wpf.Controls.ObjectEditor
{
    static class ObjectEditableItemsControlHelper
    {
        public static void SetSelect(this ObjectEditableItemsControl ic, FrameworkElement container, bool isMultiSelect = false)
        {
            if (ic == null)
            {
                return;
            }
            IEditableUIObject targetModel = GetModel(container);
            if (targetModel != null)
            {
                if (targetModel.IsSelected == true)
                {
                    return;
                }

                foreach (var item in ic.Items)
                {
                    var model = item as IEditableUIObject;
                    if (model != null)
                    {
                        if (targetModel == model)
                        {
                            model.IsSelected = true;
                        }
                        else if (isMultiSelect == false)
                        {
                            model.IsSelected = false;
                        }
                    }
                }
            }
        }

        public static List<FrameworkElement> GetSelectedContainer(this ObjectEditableItemsControl ic)
        {
            List<FrameworkElement> selectedContainers = new List<FrameworkElement>();
            if (ic != null)
            {
                foreach (var item in ic.Items)
                {
                    var model = item as IEditableUIObject;
                    if (model != null && model.IsSelected == true)
                    {
                        var container = ic.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;
                        if (container != null)
                        {
                            selectedContainers.Add(container);
                        }
                    }
                }
            }
            return selectedContainers;
        }

        public static void SetContainerVisibility(List<FrameworkElement> containers, Visibility visibility)
        {
            if (containers != null)
            {
                containers.ForEach(f => f.Visibility = visibility);
            }
        }

        public static IEditableUIObject GetModel(FrameworkElement fe)
        {
            if (fe != null)
            {
                return fe.DataContext as IEditableUIObject;
            }
            return null;
        }
    }
}
