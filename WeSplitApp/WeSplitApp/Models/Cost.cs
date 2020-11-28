//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WeSplitApp.Models
{
    using System;
    using System.Collections.Generic;
    using WeSplitApp.ViewModels;

    public partial class Cost : BaseViewModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cost()
        {
            this.Expenses = new HashSet<Expense>();
        }
    
        private int _journeyId;
        public int JourneyId { get => _journeyId; set { _journeyId = value; OnPropertyChanged(); } }


        private int _orderNumber;
        public int OrderNumber { get => _orderNumber; set { _orderNumber = value; OnPropertyChanged(); } }

        private string _content;
        public string Content { get => _content; set { _content = value; OnPropertyChanged(); } }

        public virtual Journey Journey { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Expense> Expenses { get; set; }
    }
}
