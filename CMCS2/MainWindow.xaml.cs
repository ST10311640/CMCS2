using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace CMCS2
{
    public partial class MainWindow : Window
    {
        // Temporary list to store all claims (simulating database)
        public ObservableCollection<Claim> Claims { get; set; }

        // Store uploaded file name for the current claim
        private string uploadedFileName = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
            Claims = new ObservableCollection<Claim>();
            lvClaims.ItemsSource = Claims;
        }

        // === FILE UPLOAD ===
        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Documents|*.pdf;*.docx;*.xlsx";

            if (dlg.ShowDialog() == true)
            {
                uploadedFileName = System.IO.Path.GetFileName(dlg.FileName);
                txtNotes.Text += $"\nAttached: {uploadedFileName}";
                MessageBox.Show("File uploaded successfully!",
                    "Upload Complete", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // === SUBMIT CLAIM ===
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(txtLecturerName.Text) ||
                    string.IsNullOrWhiteSpace(txtHoursWorked.Text) ||
                    string.IsNullOrWhiteSpace(txtHourlyRate.Text))
                {
                    MessageBox.Show("Please fill in all required fields.",
                        "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!double.TryParse(txtHoursWorked.Text, out double hours) ||
                    !double.TryParse(txtHourlyRate.Text, out double rate))
                {
                    MessageBox.Show("Hours Worked and Hourly Rate must be numeric values.",
                        "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                double total = hours * rate;

                // Create a new claim record
                Claim newClaim = new Claim
                {
                    ClaimID = Claims.Count + 1,
                    LecturerName = txtLecturerName.Text,
                    HoursWorked = hours,
                    HourlyRate = rate,
                    TotalAmount = total,
                    FileName = string.IsNullOrEmpty(uploadedFileName) ? "No File" : uploadedFileName,
                    Status = "Pending",
                    SubmissionDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
                };

                Claims.Add(newClaim);

                MessageBox.Show($"Claim submitted successfully!\nTotal Amount: R{total:F2}",
                    "Submission Successful", MessageBoxButton.OK, MessageBoxImage.Information);

                // Reset input fields
                txtLecturerName.Clear();
                txtHoursWorked.Clear();
                txtHourlyRate.Clear();
                txtNotes.Clear();
                uploadedFileName = string.Empty; // reset after submission
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred:\n{ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // === APPROVE CLAIM ===
        private void btnApprove_Click(object sender, RoutedEventArgs e)
        {
            if (lvClaims.SelectedItem is Claim selected)
            {
                selected.Status = "Approved";
                lvClaims.Items.Refresh();
                MessageBox.Show($"Claim #{selected.ClaimID} approved successfully.",
                    "Approved", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Please select a claim to approve.",
                    "No Claim Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // === REJECT CLAIM ===
        private void btnReject_Click(object sender, RoutedEventArgs e)
        {
            if (lvClaims.SelectedItem is Claim selected)
            {
                selected.Status = "Rejected";
                lvClaims.Items.Refresh();
                MessageBox.Show($"Claim #{selected.ClaimID} rejected.",
                    "Rejected", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Please select a claim to reject.",
                    "No Claim Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }

    // === CLAIM MODEL ===
    public class Claim
    {
        public int ClaimID { get; set; }
        public string LecturerName { get; set; }
        public double HoursWorked { get; set; }
        public double HourlyRate { get; set; }
        public double TotalAmount { get; set; }
        public string SubmissionDate { get; set; }
        public string Status { get; set; }
        public string FileName { get; set; }
    }
}
