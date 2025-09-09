using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;

namespace CMCS2
{
    public partial class MainWindow : Window
    {
        // Sample claim data for prototype
        public ObservableCollection<Claim> Claims { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Claims = new ObservableCollection<Claim>
            {
                new Claim { ClaimID = 1, LecturerName = "Lethabo Malete", HoursWorked = 10, HourlyRate = 200, Status = "Pending" },
                new Claim { ClaimID = 2, LecturerName = "Yinhla Mabasa", HoursWorked = 12, HourlyRate = 250, Status = "Pending" }
            };
            lvClaims.ItemsSource = Claims;
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Documents|*.pdf;*.docx;*.xlsx";
            if (dlg.ShowDialog() == true)
            {
                MessageBox.Show($"Selected file: {dlg.FileName}", "Upload Successful");
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Claim submitted successfully! (Prototype)", "Info");
        }

        private void btnApprove_Click(object sender, RoutedEventArgs e)
        {
            if (lvClaims.SelectedItem is Claim selected)
            {
                selected.Status = "Approved";
                lvClaims.Items.Refresh();
            }
        }

        private void btnReject_Click(object sender, RoutedEventArgs e)
        {
            if (lvClaims.SelectedItem is Claim selected)
            {
                selected.Status = "Rejected";
                lvClaims.Items.Refresh();
            }
        }
    }

    // Simple claim model for prototype
    public class Claim
    {
        public int ClaimID { get; set; }
        public string LecturerName { get; set; }
        public double HoursWorked { get; set; }
        public double HourlyRate { get; set; }
        public string Status { get; set; }
    }
}
