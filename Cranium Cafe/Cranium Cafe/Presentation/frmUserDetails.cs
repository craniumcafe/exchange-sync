using CraniumCafeConfig.Business_Logic;
using CraniumCafeConfig.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CraniumCafeConfig.Presentation
{
    public partial class frmUserDetails : Form
    {
        UserDetails userDetails = new UserDetails();

        public frmUserDetails()
        {
            InitializeComponent();
        }

        private void frmUserDetails_Load(object sender, EventArgs e)
        {

        }

        internal UserDetails ShowDialogExt()
        {
            try
            {
                DialogResult myResult = base.ShowDialog();

                if (myResult == System.Windows.Forms.DialogResult.OK)
                {
                    return userDetails;
                }
                else
                {
                    return new UserDetails();
                }
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in ShowDialogExt " + ex.Message);
                return new UserDetails();
            }
           
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                userDetails.SMTPAddress = txtSMTPAddress.Text;
                userDetails.UserID = txtUSerID.Text;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in btnOK_Click " + ex.Message);
            }

        }
    }
}
