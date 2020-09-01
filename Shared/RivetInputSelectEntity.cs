using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using MangaQuotes.Data;

//Inspired by https://www.meziantou.net/creating-a-inputselect-component-for-enumerations-in-blazor.htm
// Note that adding a constraint on T (where T : Enum) doesn't work when used in the view, Razor raises an error at build time. Also, this would prevent using nullable types...
// Because of that ambiguity we're stuck using reflection in a few places.
namespace MangaQuotes.Shared
{
    public class RivetInputSelectEntity<T> : InputSelect<T> 
    {
        private string _Id = System.Guid.NewGuid().ToString();
        private List<string> _ValidationMessages = new List<string>();

        ///<summary> The options in this select.</summary>
        [Parameter]
        public List<T> Options { get; set; }

        ///<summary> The text used in th HTML Label tag.</summary>
        [Parameter]
        public string Label { get; set; }

        ///<summary> A Func that takes one of the provided Options and returns the string to disply in the HTML option tag.</summary>
        [Parameter]
        public Func<T, string> DisplayLambda { get; set; }

        ///<summary> Information text, displayed in a smaller font, beneath the HTML Input element.</summary>
        [Parameter]
        public string Description { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if(DisplayLambda == null){
                return;
            }
            var curValueId = 0;
            if(Value != null) {
                curValueId = (int) Value.GetType().GetProperty("Id").GetValue(Value, null);
            }
            var i = -1;
            //<label>
            builder.OpenElement(i++, "label");
            builder.AddAttribute(i++, "for", _Id);
            builder.AddAttribute(i++, "class", "rvt-m-top-md");
            builder.AddContent(i++, Label);
            builder.CloseElement();

            //<select>
            builder.OpenElement(i++, "select");
            builder.AddMultipleAttributes(i++, AdditionalAttributes);
            builder.AddAttribute(i++, "Id", _Id);
            builder.AddAttribute(i++, "class", IsValid() ? CssClass : "rvt-validation-danger");
            builder.AddAttribute(i++, "aria-describedby", _Id);
            builder.AddAttribute(i++, "aria-invalid", IsValid() ? "true" : "false");
            builder.AddAttribute(i++, "value", BindConverter.FormatValue(CurrentValueAsString));
            builder.AddAttribute(i++, "onchange", EventCallback.Factory.CreateBinder<string>(this, value => CurrentValueAsString = value, CurrentValueAsString, null));
            //blank <option>
            builder.OpenElement(i++, "option");
            builder.AddContent(i++, "");
            builder.CloseElement();
            
            //<option>s
            if(Options != null)
            {
                foreach (var option in Options)
                {
                    var optionAsT = (T)Convert.ChangeType(option, typeof(T));
                    var optionId = getOptionId(optionAsT);

                    builder.OpenElement(i++, "option");
                    builder.AddAttribute(i++, "value", optionId);
                    if(optionId == curValueId){
                        builder.AddAttribute(i++, "selected", true);
                    }
                    builder.AddContent(i++, DisplayLambda(optionAsT));
                    builder.CloseElement();
                }
            }
            
            builder.CloseElement();

            //<Description>
            builder.OpenComponent<MangaQuotes.Shared.Description>(i++);
            builder.AddAttribute(i++, "Id", _Id);
            builder.AddAttribute(i++, "Value", Description);
            builder.CloseComponent();

            //<ValidationMessages>
            builder.OpenComponent<MangaQuotes.Shared.ValidationMessages>(i++);
            builder.AddAttribute(i++, "Id", _Id);
            builder.AddAttribute(i++, "Messages", _ValidationMessages);
            builder.CloseComponent();
        }
        
        protected new string CurrentValueAsString {
            get {
                return "";
            }
            set { 
                UpdateValue(value);
            }
        }

        private void UpdateValue(string id)
        {
            int selectedOptionId = 0;
            int.TryParse(id, out selectedOptionId);
            var newValue = Options.SingleOrDefault(o => {
                var x = getOptionId(o);
                return x == selectedOptionId;
            });
            var valueAsType = (T)Convert.ChangeType(newValue, typeof(T));
            ValueChanged.InvokeAsync(valueAsType).Wait();
            EditContext.NotifyFieldChanged(FieldIdentifier);
        }
        private bool IsValid()
        {
            return _ValidationMessages.Count == 0;
        }

        private int getOptionId (T option)
        {
            return (int)option.GetType().GetProperty("Id").GetValue(option);
        }
        //Add event handler for when validation state changes.
        protected override void OnInitialized()
        {
            //Throw an error if T is not an Entity.
            if(typeof(T).IsSubclassOf(typeof(Entity)) == false) {
                throw new Exception($"RivetInputSelectEntity: {typeof(T).Name} does not inherit from Models.Entity.");
            }
            //Throw an error if DisplayLambda was not provided.
            //This should help prevent some head-scratching next time we use the component.
            if(DisplayLambda == null)
            {
                throw new Exception("RivetInputSelectEntity: DisplayLambda is a required parameter.");
            }

            //Subscribe to the OnValidationStateChanged event.
            base.EditContext.OnValidationStateChanged += ValidationStateChanged;
        }

        private void ValidationStateChanged(object sender, ValidationStateChangedEventArgs e)
        {
            _ValidationMessages = this.EditContext.IsModified() ? this.EditContext.GetValidationMessages(this.FieldIdentifier).ToList() : new List<string> ();
        }
        
        public void Dispose()
        {
            base.EditContext.OnValidationStateChanged -= ValidationStateChanged;
        }
        //End of validation event handling.
    }
}