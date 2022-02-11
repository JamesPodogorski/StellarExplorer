
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components;
// using Microsoft.JSInterop;

using playground.Data;

namespace playground.Pages;


public partial class TreeviewBase<T>: ComponentBase where T : FarmViewBase
    {

        [Parameter]
        public List<T> DataSource { get; set; }
        [Parameter]
        public string Id { get; set; }
        [Parameter]
        public string ParentId { get; set; }
        [Parameter]
        public string HasChildren { get; set; }
        [Parameter]
        public string Text { get; set; }
        [Parameter]
        public string Expanded { get; set; }

        protected List<T> AllItems;
        protected Dictionary<int, bool> _caretDown= new Dictionary<int, bool>();
        protected Dictionary<int, string> _caretcss=new Dictionary<int,string>();
        protected Dictionary<int, string> _nestedcss=new Dictionary<int,string>();
       
        protected override Task OnInitializedAsync()
        {
            //asigning to its new instance to avoid exceptions.
            AllItems = new List<T>();
            AllItems = DataSource.ToArray().ToList();

            if (AllItems != null)
            {
                foreach (var item in AllItems)
                {
                    //  var _id = Convert.ToInt32(GetPropertyValue(item, Id));
                      var _id = item.Id;
                    
                    //initializing fields with default value.
                    _caretDown.Add(_id, true);
                    _caretcss.Add(_id, "caret");
                    _nestedcss.Add(_id, "nested");
                }

            }
            return base.OnInitializedAsync();
        }


        protected override Task OnParametersSetAsync()
        {
            //This will check if the first item in the
            // list/collection has a "parentId" then remove the "parentId" from it. 
            //Because we use the first item as a reference in the razor file, so it must not have "parentId".

            var Parem = AllItems.First();
            Parem.ParentId = string.Empty;
            // switch (GetPropertyType(Parem, ParentId))
            // switch (Parem.ParentId)
            // {
            //     case "Int32":
            //         if (Convert.ToInt32(GetPropertyValue(Parem, ParentId)) > 0)
            //         {
            //             SetPropertyValue<int>(Parem, ParentId, 0);
            //         }
            //         break;
            //     case "String":
            //         if (GetPropertyValue(Parem, ParentId) != "")
            //         {
            //             SetPropertyValue<string>(Parem, ParentId, "");
            //         }

            //         break;
            //     default:
            //         break;
            // }

            return base.OnParametersSetAsync();
        }



        protected void SpanToggle(T item)
        {
        //    var _clckdItemid = Convert.ToInt32(GetPropertyValue(item, Id));
           var _clckdItemid = item.Id;

            _caretcss[_clckdItemid] = _caretDown[_clckdItemid] ? "caret caret-down" : "caret";
            _nestedcss[_clckdItemid] = _caretDown[_clckdItemid] ? "active" : "nested";
            _caretDown[_clckdItemid] = !_caretDown[_clckdItemid];
        }

        #region reflection methodes to get your property type, propert value and also set property value 
        protected string GetPropertyValue(T item, string Property)
        {

            if (item != null)
            {
                return item.GetType().GetProperty(Property).GetValue(item, null).ToString();
            }
            return "";

        }

        protected void SetPropertyValue<T>(T item, string Property, T value)
        {
            if (item != null)
            {
                item.GetType().GetProperty(Property).SetValue(item, value);
            }
        }

        protected string GetPropertyType(T item, string Property)
        {

            if (item != null)
            {
           return item.GetType().GetProperty(Property).PropertyType.Name;

            }
            return null;
        }
        #endregion
    }
