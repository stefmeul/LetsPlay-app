using LetsPlayClassLibrary;
using System.Windows;
using System.Windows.Controls;

namespace WpfLetsPlayApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int LoggedInUserId = -1;

        public MainWindow()
        {
            InitializeComponent();
            CollapseAllGrids();
        }

        // method to set main menu grid visibility
        private void SetGridVisibility(int menu, bool visible)
        {
            Grid grid = (Grid)FindName($"grdPart{menu}");
            if (grid != null)
            {
                grid.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        // method to collapse all grid
        private void CollapseAllGrids()
        {
            foreach (ListBoxItem item in lsbMenu.Items)
            {
                int menu = Convert.ToInt32(item.Tag);
                SetGridVisibility(menu, false);
            }
        }

        // method to display all Projects
        public void DisplayProjects()
        {
            // clear listbox
            lsbProjects.Items.Clear();

            if (Model.Projects != null)
            {
                int tagNr = 1;

                foreach (Project pr in Model.Projects)
                {
                    lsbProjects.Items.Add(new ListBoxItem()
                    {
                        Content = $"{pr.ProjectId} {pr.Title} {pr.Budget}",
                        Tag = tagNr
                    });

                    tagNr++;
                }
            }
        }

        // method to display all Accounts
        public void DisplayAccounts()
        {
            // clear labels
            lsbProjects.Items.Clear();
            lblHighestBalance.Content = string.Empty;
            lblLowestBalance.Content = string.Empty;
            lblUserBalance.Content = string.Empty;
            lblNumberOfUsers.Content = string.Empty;

            // add accounts balances to list Balances
            if (Model.Accounts != null)
            {
                // clear balance list
                Model.Balances.Clear();

                foreach (Account a in Model.Accounts)
                {
                    Model.Balances.Add(a.Balance);

                    // set User Balance label of LoggedInUserId
                    if(LoggedInUserId == a.UserId)
                    {
                        lblUserBalance.Content = a.Balance.ToString("0.00");
                    }
                }
            }

            // set lables
            lblHighestBalance.Content = Model.Balances.Max().ToString("0.00");
            lblLowestBalance.Content = Model.Balances.Min().ToString("0.00");
            lblNumberOfUsers.Content = Model.Balances.Count().ToString("0");
      
        }

        private void lsbMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            CollapseAllGrids();

            if (lsbMenu.SelectedIndex >= 0)
            {
                int menu = Convert.ToInt32(((ListBoxItem)lsbMenu.SelectedItem).Tag);
                if (menu > 0)
                {
                    SetGridVisibility(menu, true);
                }
                // Accounts selected
                if (menu == 2)
                {
                    // load Users and Accounts from db
                    Model.Users = User.LoadUsers();
                    Model.Accounts = Account.LoadAccounts();
                    DisplayAccounts();
                }
                // Projects selected
                if (menu == 3)
                {
                    // load projects from db
                    Model.Projects = Project.LoadProjects();
                    DisplayProjects();
                }
                // logout selected
                if (menu == 4)
                {
                    LoggedInUserId = -1;
                    lsbMenu.SelectedIndex = 0;
                    
                }
            }
        }

        // register new user
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            User user = new User();
            user.Email = txtEmail.Text;
            user.Password = pwbPassword.Password;
            user.RegisterUser();

            if (user.LoginUser())
            {
                txtEmail.Text = string.Empty;
                pwbPassword.Password = string.Empty;
                LoggedInUserId = user.UserId;
                lsbMenu.SelectedIndex = 1;
            }
            else
            {
                lblError.Content = "invalid email or password";
            }
        }

        // login user
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

            User user = new User();
            user.Email = txtEmail.Text;
            user.Password = pwbPassword.Password;
            lblError.Content = string.Empty;

            if (user.LoginUser())
            {
                txtEmail.Text = string.Empty;
                pwbPassword.Password = string.Empty;
                LoggedInUserId = user.UserId;
                lsbMenu.SelectedIndex = 1;
            }
            else
            {
                lblError.Content = "invalid email or password";
            }

        }

        private void btnBuy_Click(object sender, RoutedEventArgs e)
        {
            // update balance and redistribute
            decimal amount = Convert.ToDecimal(txtBuy.Text); // add point to comma conversion

            Account.Redistribute(amount, LoggedInUserId);
            DisplayAccounts();


        }

        private void btnDonate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnMake_Click(object sender, RoutedEventArgs e)
        {
            // make new project
        }
    }
}