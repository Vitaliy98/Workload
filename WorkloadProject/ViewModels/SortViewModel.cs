using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkloadProject.Models
{
    public class SortViewModel
    {
        public SortState FirstNameSort { get; private set; }
        public SortState MiddleNameSort { get; private set; }
        public SortState SurnameSort { get; private set; }
        public SortState PositionSort { get; private set; }    
        public SortState DateSort { get; private set; }
        public SortState TimeSort { get; private set; }
        public SortState SubjectSort { get; private set; }
        public SortState Current { get; private set; }     

        public SortViewModel(SortState sortOrder)
        {
            FirstNameSort = sortOrder == SortState.FirstNameAsc ? SortState.FirstNameDesc : SortState.FirstNameAsc;
            MiddleNameSort = sortOrder == SortState.MiddleNameAsc ? SortState.MiddleNameDesc : SortState.MiddleNameAsc;
            SurnameSort = sortOrder == SortState.SurnameAsc ? SortState.SurnameDesc : SortState.SurnameAsc;
            PositionSort = sortOrder == SortState.PositionAsc ? SortState.PositionDesc : SortState.PositionAsc;
            DateSort = sortOrder == SortState.DateAsc ? SortState.DateDesc : SortState.DateAsc;
            TimeSort = sortOrder == SortState.TimeAsc ? SortState.TimeDesc : SortState.TimeAsc;
            SubjectSort = sortOrder == SortState.SubjectAsc ? SortState.SubjectDesc : SortState.SubjectAsc;
            Current = sortOrder;
        }
    }
}
