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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Article()
        {
            //this.UsersArticles = new HashSet<UsersArticle>();
        }
    
        public int Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public System.DateTime Date { get; set; }
        public int IdNewsProvider { get; set; }
        public string Image { get; set; }
    
        public virtual NewsProvider NewsProvider { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UsersArticle> UsersArticles { get; set; }

        public Article CloneClient(Article art)
        {
            return new Article {Date = art.Date,Id = art.Id, Description = art.Description, Image = art.Image, Link = art.Link, Title = art.Title, IdNewsProvider = art.IdNewsProvider };
        }
    }
}
