//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebDBSchema.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class InvPoApproval
    {
        public int Id { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public string VerifiedBy { get; set; }
        public Nullable<System.DateTime> VerifiedDate { get; set; }
        public string EncodedBy { get; set; }
        public System.DateTime EncodedDate { get; set; }
        public int InvPoHdrId { get; set; }
    
        public virtual InvPoHdr InvPoHdr { get; set; }
    }
}