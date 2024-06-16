using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorDemand.Data.Dtos.Subject;

namespace TutorDemand.Data.Utils
{
    public class SortingHelper
    {
        public static IEnumerable<SubjectDto> SortSubjectsByColumn(IEnumerable<SubjectDto> paginatedSubjects, string sort)
        {
            string sortOrder = sort.TrimStart('+', '-');
            bool descendingOrder = sort.StartsWith("-");

            var sortMappings = new Dictionary<string, Func<SubjectDto, object>>
                {
                    { "CODE", p => p.SubjectCode },
                    { "NAME", p => p.Name },
                    { "STARTDATE", p =>
                        {
                            return p.StartDate.HasValue ? p.StartDate.Value : null!;
                        }
                    },
                    { "ENDDATE", p =>
                        {
                            return p.EndDate.HasValue ? p.EndDate.Value : null!;
                        }
                    },
                    { "PRICE", p => p.CostPrice }
                };

            if (sortMappings.TryGetValue(sortOrder.ToUpper(), out var sortExpression))
            {
                return descendingOrder
                    ? paginatedSubjects.OrderByDescending(sortExpression)
                    : paginatedSubjects.OrderBy(sortExpression);
            }
            else
            {
                return paginatedSubjects;
            }
        }

        // Other sorting here...
    }
}
