using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TermPlanner.Models
{
    public class Assessment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime DueDate { get; set; }
        public bool AlertOn { get; set; }

    }
}
