using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TermPlanner.Models
{
    public class Term
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
