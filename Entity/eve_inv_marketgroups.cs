//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class eve_inv_marketgroups
    {
        public int marketgroup_id { get; set; }
        public Nullable<int> parentgroup_id { get; set; }
        public string marketgroup_name { get; set; }
        public string description { get; set; }
        public Nullable<int> has_types { get; set; }
        public int parent_id_1 { get; set; }
        public int parent_id_2 { get; set; }
        public int parent_id_3 { get; set; }
        public int parent_id_4 { get; set; }
        public int parent_id_5 { get; set; }
        public string parent_text { get; set; }
    }
}