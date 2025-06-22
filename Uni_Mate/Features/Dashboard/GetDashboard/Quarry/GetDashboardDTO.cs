namespace Uni_Mate.Features.Dashboard.GetDashboard.Quarry
{
    public class GetDashboardDTO
{
    // 🏢 الشقق
    public int TotalApartments { get; set; }
    public int AvailableApartments { get; set; }
    public int BookedApartments { get; set; }

    // 👤 الطلاب
    public int TotalStudents { get; set; }
    public int StudentsBooked { get; set; }
    public int StudentsNotBooked { get; set; }

    // 👨‍💼 ملاك العقارات
    public int TotalOwners { get; set; }

    // ✅ الطلبات
    public int TotalBookings { get; set; }
    public int ApprovedBookings { get; set; }
    public int RejectedBookings { get; set; }
    public int PendingBookings { get; set; }

    // 💬 التعليقات
    public int TotalComments { get; set; }
}

}
