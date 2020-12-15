using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WeSplitApp.Model;
using WeSplitApp.Models;

namespace WeSplitApp.ViewModels
{
    class MembersUCViewModel : BaseViewModel
    {
        #region Properties

        private AsyncObservableCollection<dynamic> _memberList;
        public AsyncObservableCollection<dynamic> MemberList { get => _memberList; set { _memberList = value; OnPropertyChanged(); } }

        private dynamic _selectedMember;

        public dynamic SelectedMember { get => _selectedMember; set {
                _selectedMember = value;
                if (SelectedMember != null)
                {
                    Member member = DataProvider.Ins.DB.Members.Find(SelectedMember.Id);
                    MemberName = member.Name;
                    MemberPhone = member.Phone;
                    MemberEmail = member.Email;
                    ImageCover = (member.ImageBytes != null) ? member.ImageBytes : System.Text.Encoding.Default.GetBytes(Global.GetInstance().NoImageStringSource);
                    UpdateOrAddContent = "SỬA";
                }
                else
                {
                    MemberName = null;
                    MemberPhone = null;
                    MemberEmail = null;
                    ImageCover = null;
                    UpdateOrAddContent = "THÊM";

                }
                OnPropertyChanged();
            }
        }

        private string _memberName;
        public string MemberName 
        { 
            get => _memberName; 
            set 
            { 
                _memberName = value; 
                if (MemberName != null && MemberPhone != null && MemberEmail != null && IsPhoneValid == true && IsEmailValid == true)
                {
                    IsEnabledAddMemberButton = true;
                }
                IsNameValid = true;
                OnPropertyChanged(); 
            } 
        }

        private string _memberPhone;
        public string MemberPhone 
        {
            get => _memberPhone; 
            set
            { 
                _memberPhone = value;
                if (MemberName != null && MemberPhone != null && MemberEmail != null && IsNameValid == true && IsEmailValid == true)
                {
                    IsEnabledAddMemberButton = true;
                }
                IsPhoneValid = true;
                OnPropertyChanged();
            } 
        }

        private string _memberEmail;
        public string MemberEmail 
        { 
            get => _memberEmail; 
            set
            { 
                _memberEmail = value;
                if (MemberName != null && MemberPhone != null && MemberEmail != null && IsNameValid == true && IsPhoneValid == true)
                {
                    IsEnabledAddMemberButton = true;
                }
                IsEmailValid = true;
                OnPropertyChanged();
            }
        }

        private byte[] _imageCover;
        public byte[] ImageCover { get => _imageCover; set { _imageCover = value; OnPropertyChanged(); } }

        private string _updateOrAddContent;
        public string UpdateOrAddContent { get => _updateOrAddContent; set { _updateOrAddContent = value; OnPropertyChanged(); } }
        #endregion

        #region IsValidValue
        private bool _isNameValid;
        public bool IsNameValid { get => _isNameValid; set { _isNameValid = value; OnPropertyChanged(); } }
        private bool _isEmailValid;
        public bool IsEmailValid { get => _isEmailValid; set { _isEmailValid = value; OnPropertyChanged(); } }
        private bool _isPhoneValid;
        public bool IsPhoneValid { get => _isPhoneValid; set { _isPhoneValid = value; OnPropertyChanged(); } }
        #endregion

        private bool _isEnabledAddMemberButton;
        public bool IsEnabledAddMemberButton { get => _isEnabledAddMemberButton; set { _isEnabledAddMemberButton = value; OnPropertyChanged(); } }

        #region Command
        public ICommand AddImageCommand { get; set; }
        public ICommand AddMemberCommand { get; set; }
        public ICommand DeleteMemberCommand { get; set; }
        public ICommand DisableName { get; set; }
        public ICommand DisableEmail { get; set; }
        public ICommand DisablePhone { get; set; }
        #endregion

        public MembersUCViewModel()
        {
            UpdateOrAddContent = "THÊM";

            MemberList = new AsyncObservableCollection<dynamic>();
            foreach (var member in DataProvider.Ins.DB.Members)
            {
                AddMemberToViewList(member);
            }

            AddImageCommand = new RelayCommand<object>((param) => { return true; }, (param) => {
                string AbsoluteLink = "";
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Multiselect = false;
                dialog.Filter = "JPG files (*.jpg)|*.jpg| PNG files (*.png)|*.png| All files (*.*)|*.*";
                if (dialog.ShowDialog() == true)
                {
                    AbsoluteLink = dialog.FileName;
                    ImageCover = BitMapImageTOBytes(new BitmapImage(
                    new Uri(AbsoluteLink,
                    UriKind.Absolute)
                    ));
                }
            });

            AddMemberCommand = new RelayCommand<object>((param) => { return true; }, (param) => {
                if (SelectedMember != null) // update Member
                {
                    var updateMember = DataProvider.Ins.DB.Members.Find(SelectedMember.Id);
                    updateMember.Name = MemberName;
                    updateMember.Phone = MemberPhone;
                    updateMember.Email = MemberEmail;
                    updateMember.ImageBytes = ImageCover;
                    DataProvider.Ins.DB.SaveChanges();
                    MemberList = new AsyncObservableCollection<dynamic>();
                    foreach (var member in DataProvider.Ins.DB.Members.OrderBy(x => x.Id))
                    {
                        AddMemberToViewList(member);
                    }
                }
                else // Add new Member
                {
                    Member newMember = new Member()
                    {
                        Name = MemberName,
                        Phone = MemberPhone,
                        Email = MemberEmail,
                        ImageBytes = ImageCover,
                    };
                    DataProvider.Ins.DB.Members.Add(newMember);
                    DataProvider.Ins.DB.SaveChanges();
                    AddMemberToViewList(newMember);
                }
                SelectedMember = null;
                IsEnabledAddMemberButton = false;
            });

            DeleteMemberCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                int id = param.Id;
                Member deleteItem = DataProvider.Ins.DB.Members.Find(id);
                if (deleteItem.Journeys.Count > 0)
                {
                    string listplace = "";
                    foreach (var journey in deleteItem.Journeys)
                    {
                        listplace += "- " + journey.Name + "\n";
                    }
                    MessageBox.Show("Hiện tại người này đang tham gia những chuyến đi sau\n\n" + listplace + "\nBạn cần xóa sự tham gia của họ để xóa dữ liệu thành viên này.");
                }
                else
                {
                    if (Global.GetInstance().ConfirmMessageDelete() == true)
                    {
                        DataProvider.Ins.DB.Members.Remove(deleteItem);
                        MemberList.Remove(param);
                    }
                }
            });
            DisableName = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                IsNameValid = false;
                IsEnabledAddMemberButton = false;
            });
            DisableEmail = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                IsEmailValid = false;
                IsEnabledAddMemberButton = false;
            });
            DisablePhone = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                IsPhoneValid = false;
                IsEnabledAddMemberButton = false;
            });

        }


        public byte[] BitMapImageTOBytes(BitmapImage imageC)
        {
            if (imageC == null) return null;
            MemoryStream memStream = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageC));
            encoder.Save(memStream);
            return memStream.ToArray();
        }

        public void AddMemberToViewList(Member member)
        {
            dynamic tmp = new
            {
                Id = member.Id,
                Name = member.Name,
                Phone = member.Phone,
                Email = member.Email,
                Avatar = (member.ImageBytes != null) ? member.ImageBytes : System.Text.Encoding.Default.GetBytes(Global.GetInstance().NoImageStringSource),
            };
            MemberList.Add(tmp);
        }
    }
    public class IsEmailFormatRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                if (((string)value).Length > 0)
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch((string)value, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                    {
                        return ValidationResult.ValidResult;
                    }
                    else
                    {
                        return new ValidationResult(false, "Vui lòng nhập trường này đúng định dạng email");
                    }
                }
                else
                {
                    return new ValidationResult(false, "Vui lòng không bỏ trống trường này");
                }
            }
            catch (Exception e)
            {
                return new ValidationResult(false, e.Message);
            }

        }
    }
}

