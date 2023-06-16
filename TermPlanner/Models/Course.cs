using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TermPlanner.Models
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int TermId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool AlertOn { get; set; }
        public string InstrName { get; set; }
        public string InstrPhone { get; set; }
        public string InstrEmail { get; set; }
        public string Notes { get; set; }

    }
}
