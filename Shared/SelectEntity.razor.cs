using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace MangaQuotes.Shared
{
    public partial class SelectEntity<T> : InputBase<T>
    {
        ///<summary> The options in this select.</summary>
        [Parameter]
        public List<T> Options { get; set; }

        ///<summary> The text used in th HTML Label tag.</summary>
        [Parameter]
        public string Label { get; set; }

        private string StringValue { get; set; }
        private List<Option> FormattedOptions => GetOptions();

        protected override void OnInitialized()
        {
            //Throw an error if T is not an Entity.
            if(typeof(T).IsSubclassOf(typeof(MangaQuotes.Data.Entity)) == false) {
                throw new Exception($"{typeof(T).Name} does not inherit from MangaQuotes.Data.Entity.");
            }
        }

        private List<Option> GetOptions()
        {
            string valueId = Value == null ? "" : ((int)Value.GetType().GetProperty("Id").GetValue(Value)).ToString();
            var output = new List<Option> { new Option{ Id = "0", DisplayName = "", Selected = (valueId == "0" || valueId == "")} };

            foreach(var o in Options)
            {
                var tOption = new Option
                {
                    Id = ((int)o.GetType().GetProperty("Id").GetValue(o)).ToString(),
                    DisplayName = (string)o.GetType().GetProperty("Name").GetValue(o)
                };
                tOption.Selected = valueId == tOption.Id;

                output.Add(tOption);
            }

            return output;
        }

        private void SelectedOptionChanged (ChangeEventArgs evt)
        {
            Console.WriteLine($"FRom SelectedOptionChanged() {StringValue} vs. {evt.Value}");
            foreach(var o in Options)
            {
                if(((int)o.GetType().GetProperty("Id").GetValue(o)).ToString() == (string)evt.Value)
                {
                    ValueChanged.InvokeAsync(o).Wait();
                    return;
                }
            }

            ValueChanged.InvokeAsync(default(T));
        }

        protected override bool TryParseValueFromString(string value, out T result, out string validationErrorMessage)
        {
            throw new System.NotImplementedException();
        }

        private class Option
        {
            public string Id { get; set; }
            public string DisplayName { get; set; }
            public bool Selected { get; set; }
        }
    }
}