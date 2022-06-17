using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace Chat
{
    [Table(Name ="messages")]
   public class Massege
    {
        [Column(IsDbGenerated = true, IsPrimaryKey = true, Name = "id")]
        public int Id { get; set; }
        [Column(Name = "text")]
        public string Text { get; set; }
        [Column(Name = "username")]
        public string Username { get; set; }
        [Column(Name = "date")]
        public DateTime Date { get; set; }
    }
}
