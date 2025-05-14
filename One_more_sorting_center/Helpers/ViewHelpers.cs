using One_more_sorting_center.Models;

namespace One_more_sorting_center.Helpers
{
    public static class ViewHelpers
    {
        public static string GetStatusBadgeClass(OrderStatus status)
        {
            return status switch
            {
                OrderStatus.InProgress => "bg-warning",
                OrderStatus.Completed => "bg-success",
                OrderStatus.Cancelled => "bg-danger",
                _ => "bg-secondary"
            };
        }

        public static string GetProblemTypeBadgeClass(IssueType issue)
        {
            return issue switch
            {
                IssueType.Damaged => "bg-danger",
                IssueType.StuckInStorage => "bg-primary",
                _ => "bg-light text-dark"
            };
        }
    }
}