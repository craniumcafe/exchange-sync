using CraniumCafeConfig.Business_Logic;
using CraniumCafeConfig.Models;
using CraniumCafeConfig.Presentation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CraniumCafeConfig
{
    public partial class frmConfiguration : Form
    {
        private BackgroundWorker bw;

        public frmConfiguration()
        {
            InitializeComponent();
        }  

        private void btnAddnewuser_Click(object sender, EventArgs e)
        {
            try
            {
                frmUserDetails userdetail = new frmUserDetails();
                UserDetails newuser = userdetail.ShowDialogExt();

                //check userid alreday added 
                if (IsUserAllreadyAdd(newuser))
                {
                    MessageBox.Show(newuser.UserID + " this user id already added ", "CraniumCafe add user");
                }
                else
                {
                    BindData(newuser);
                }
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in btnAddnewuser_Click " + ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveConfigDetails();
        }

        private void BindData(UserDetails newuser)
        {
            try
            {
                List<UserDetails> existingUsers = GetExistingUser();

                DataTable table = new DataTable();

                table.Columns.Add("Exchange SMTP Address", typeof(string));
                table.Columns.Add("Cranium User ID", typeof(string));
                table.Columns.Add("Action", typeof(Image));

                //new             
                table.Rows.Add(newuser.SMTPAddress, newuser.UserID, Properties.Resources.Remove_1616);

                //existing
                foreach (UserDetails user in existingUsers)
                {
                    table.Rows.Add(user.SMTPAddress, user.UserID, Properties.Resources.Remove_1616);
                }

                dgvUserCredential.DataSource = table;
                dgvUserCredential.Columns[0].Width = 290;
                dgvUserCredential.Columns[1].Width = 200;
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in BindData " + ex.Message);
            }
        }

        private void BindDataWithConfigFile(List<UserDetails> userlist)
        {
            try
            {
                DataTable table = new DataTable();
                table.Columns.Add("Exchange SMTP Address", typeof(string));
                table.Columns.Add("Cranium User ID", typeof(string));
                table.Columns.Add("Action", typeof(Image));

                //existing
                foreach (UserDetails user in userlist)
                {
                    table.Rows.Add(user.SMTPAddress, user.UserID, Properties.Resources.Remove_1616);
                }

                dgvUserCredential.DataSource = table;
                dgvUserCredential.Columns[0].Width = 290;
                dgvUserCredential.Columns[1].Width = 200;
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in BindDataWithConfigFile " + ex.Message);
            }
        }

        private List<UserDetails> GetExistingUser()
        {
            try
            {
                List<UserDetails> existingUsers = new List<UserDetails>();
                foreach (DataGridViewRow row in dgvUserCredential.Rows)
                {
                    try
                    {
                        UserDetails user = new UserDetails();
                        user.SMTPAddress = row.Cells["Exchange SMTP Address"].Value.ToString();
                        user.UserID = row.Cells["Cranium User ID"].Value.ToString();
                        existingUsers.Add(user);
                    }
                    catch (Exception ex)
                    {
                        Debug.DebugMessage(2, "Error in GetExistingUser " + ex.Message);
                    }
                }

                return existingUsers;
            }
            catch (Exception ex1)
            {
                Debug.DebugMessage(2, "Error in GetExistingUser " + ex1.Message);
                return null;
            }
        }

        private void frmConfiguration_Load(object sender, EventArgs e)
        {
            Globals.SetFilePaths();
            SetConfigForm();
            txtKey.Focus();
            txtKey.Select();
        }

        private void SetConfigForm()
        {
            try
            {
                EnDecrypt enDecryptObj = new EnDecrypt();
                CraniumConfig craniumConfig = new CraniumJsonConfig().GetCraniumConfig();
                if (craniumConfig != null)
                {
                    txtKey.Text = enDecryptObj.Decrypt(craniumConfig.RestKey);
                    txtSecret.Text = enDecryptObj.Decrypt(craniumConfig.RestSecret);
                    txtAgentUsername.Text = enDecryptObj.Decrypt(craniumConfig.AgentUsername);
                    //txtAgentPassword.Text = enDecryptObj.Decrypt(craniumConfig.AgentPassword);
                    txtServerName.Text = enDecryptObj.Decrypt(craniumConfig.ExcServerName);
                    txtInterval.Text = craniumConfig.PoolingInterval;
                    BindDataWithConfigFile(craniumConfig.UserDetailsList);
                }
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in SetConfigForm " + ex.Message);
            }
        }

        private void dgvUserCredential_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 2)
                {
                    dgvUserCredential.Rows.RemoveAt(e.RowIndex);
                }
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in dgvUserCredential_CellClick " + ex.Message);
            }
        }

        private void frmConfiguration_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (IsAnyChangeToSave())
                {
                    DialogResult dr = MessageBox.Show("Do you want save changes?", "Cranium Cafe Configuration", MessageBoxButtons.YesNoCancel,
                 MessageBoxIcon.Information);

                    if (dr == DialogResult.Yes)
                    {
                        SaveConfigDetails();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in frmConfiguration_FormClosed " + ex.Message);
            }
        }

        private bool IsAnyChangeToSave()
        {
            bool isUnsave = false;
            try
            {
                EnDecrypt enDecryptObj = new EnDecrypt();
                CraniumConfig craniumConfig = new CraniumJsonConfig().GetCraniumConfig();

                if (craniumConfig != null)
                {
                    if (enDecryptObj.Decrypt(craniumConfig.RestKey) != txtKey.Text.Trim())
                        isUnsave = true;
                    else if (enDecryptObj.Decrypt(craniumConfig.RestSecret) != txtSecret.Text.Trim())
                        isUnsave = true;
                    else if (enDecryptObj.Decrypt(craniumConfig.AgentUsername) != txtAgentUsername.Text.Trim())
                        isUnsave = true;
                    else if (enDecryptObj.Decrypt(craniumConfig.AgentPassword) != txtAgentPassword.Text.Trim())
                        isUnsave = true;
                    else if (craniumConfig.PoolingInterval != txtInterval.Text.Trim())
                        isUnsave = true;
                    else
                    {
                        // need to check grid 
                        isUnsave = AnyChangesOnGrid(craniumConfig.UserDetailsList);
                    }
                }
                else
                {
                    // need to check anyvalu in form 
                    isUnsave = IsEnterAnyValues();
                }
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in IsAnyChangeToSave " + ex.Message);
            }
            return isUnsave;
        }

        private bool AnyChangesOnGrid(List<UserDetails> jsonUserDetails)
        {
            bool isChange = false;
            try
            {
                List<UserDetails> gridUserDetails = GetExistingUser();

                foreach (UserDetails userJason in jsonUserDetails)
                {
                    var res = gridUserDetails.Find(x => x.SMTPAddress == userJason.SMTPAddress && x.UserID == userJason.UserID);
                    if (res == null)
                        isChange = true;
                }

                foreach (UserDetails userGrid in gridUserDetails)
                {
                    var res = jsonUserDetails.Find(x => x.SMTPAddress == userGrid.SMTPAddress && x.UserID == userGrid.UserID);
                    if (res == null)
                        isChange = true;
                }

            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in AnyChangesOnGrid " + ex.Message);
            }

            return isChange;
        }

        private bool IsEnterAnyValues()
        {
            bool isUnsave = false;

            try
            {
                if (!string.IsNullOrEmpty(txtKey.Text.Trim()))
                    isUnsave = true;
                else if (!string.IsNullOrEmpty(txtSecret.Text.Trim()))
                    isUnsave = true;
                else if (!string.IsNullOrEmpty(txtAgentUsername.Text.Trim()))
                    isUnsave = true;
                else if (!string.IsNullOrEmpty(txtAgentPassword.Text.Trim()))
                    isUnsave = true;
                else if (!string.IsNullOrEmpty(txtInterval.Text.Trim()))
                    isUnsave = true;
                else if (dgvUserCredential.Rows.Count > 0)
                    isUnsave = true;
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in IsEnterAnyValues " + ex.Message);
            }

            return isUnsave;
        }

        private void SaveConfigDetails()
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                EnDecrypt enDecryptObj = new EnDecrypt();
                CraniumConfig craniumConfig = new CraniumConfig();
                craniumConfig.RestKey = enDecryptObj.Encrypt(txtKey.Text.Trim());
                craniumConfig.RestSecret = enDecryptObj.Encrypt(txtSecret.Text.Trim());
                craniumConfig.AgentUsername = enDecryptObj.Encrypt(txtAgentUsername.Text.Trim());
                craniumConfig.AgentPassword = enDecryptObj.Encrypt(txtAgentPassword.Text.Trim());
                craniumConfig.ExcServerName= enDecryptObj.Encrypt(txtServerName.Text.Trim());
                craniumConfig.PoolingInterval = txtInterval.Text;
                craniumConfig.UserDetailsList = GetExistingUser();

                new CraniumJsonConfig().SaveCraniumConfig(craniumConfig);
                MessageBox.Show("Sucessfully saved CraniumCafe configuration details.");
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in SaveConfigDetails " + ex.Message);
            }

            Cursor.Current = Cursors.Default;
        }

        private bool IsUserAllreadyAdd(UserDetails newuser)
        {
            bool isUserExist = false;
            try
            {
                List<UserDetails> existingUsers = GetExistingUser();
                var res = existingUsers.Find(x => x.UserID == newuser.UserID);
                if (res != null)
                    isUserExist = true;
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in IsUserAllreadyAdd " + ex.Message);
            }
            return isUserExist;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveConfigDetails();
        }

        private void btnTestRest_Click(object sender, EventArgs e)
        {
            TestAPIAndSecret();
        }

        private void TestAPIAndSecret()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                string apiKey = txtKey.Text.Trim();
                string security = txtSecret.Text.Trim();

                if (!string.IsNullOrEmpty(apiKey) && !string.IsNullOrEmpty(security))
                {
                    string userID = GetUserID();
                    if (!string.IsNullOrEmpty(apiKey))
                    {
                        string res = new CCAPIRequest().CheckMeeting(apiKey, security, userID);
                        MessageBox.Show(res, "Test Cranium Cafe Rest Key");
                    }
                    else
                    {
                        MessageBox.Show("Please add atleast one user to test! ", "Test Cranium Cafe Rest Key");
                    }
                }
                else
                {
                    MessageBox.Show("Please enter Rest key and Secret Key! ", "Test Cranium Cafe Rest Key");
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in TestAPIAndSecret " + ex.Message);
            }
        }

        private string GetUserID()
        {
            string userID = string.Empty;
            try
            {
                foreach (DataGridViewRow row in dgvUserCredential.Rows)
                {
                    try
                    {
                        userID = row.Cells["Cranium User ID"].Value.ToString();
                    }
                    catch (Exception ex1)
                    {
                        Debug.DebugMessage(2, "Error in GetUserID(ex1) " + ex1.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in GetUserID " + ex.Message);
            }

            return userID;
        }      

        private void btnTestExchange_Click(object sender, EventArgs e)
        {
            if (RedemptionCode.InitialiseRedemption(txtAgentUsername.Text.Trim(), txtServerName.Text.Trim()))
            {
                MessageBox.Show("Succesfully connected to exchange server");
            }
            else
            {
                MessageBox.Show("An error occured when trying to connect to exchange server", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
