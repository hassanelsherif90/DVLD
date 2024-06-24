using DVLD.Properties;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Controls
{
    public partial class ctrlUserCard : UserControl
    {
        
        private clsUser _User;

        private int _UserID = -1;

        public int UserID
        {
            get { return _UserID; }
        }

        public ctrlUserCard()
        {
            InitializeComponent();
        }

        public void LoadUserInfo(int UserID)
        {
            // Check Find User in the DataBase 
            _User = clsUser.FindByUserID (UserID);


            // Check User Is Null OR Not Null
            if (_User == null)
            {

                _ResetPersonInfo();

                MessageBox.Show("No User with UserID = " + UserID.ToString(),
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
           
                _FillUserInfo();
        }

        private void _FillUserInfo()
        {

            ctrlPersonCard1.LoadPersonInfo(_User.PersonID);

            lblUserID.Text   =  _User.UserID.ToString();
            lblUserName.Text =  _User.UserName.ToString();
            
            // Check User Is Active OR Not Active 

            if (_User.IsActive)
                 lblIsActive.Text = "Yes";
            else
                lblIsActive.Text = "No";

        }

        private void _ResetPersonInfo()
        {
            
            ctrlPersonCard1.ResetPersonInfo();
            lblUserID.Text = "[???]";
            lblUserName.Text = "[???]";
            lblIsActive.Text = "[???]";
        }
    }
}
