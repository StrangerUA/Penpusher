//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Penpusher
{
    using System;
    using System.Collections.Generic;
    
    public partial class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public System.DateTime Date { get; set; }
        public int IdProvider { get; set; }
    
        public virtual User Users { get; set; }
    }
}