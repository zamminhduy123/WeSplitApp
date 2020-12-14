using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WeSplitApp.Model;
using WeSplitApp.Models;

namespace WeSplitApp.ViewModels
{
    class MembersUCViewModel : BaseViewModel
    {
        //Properties 

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
        public string MemberName { get => _memberName; set { _memberName = value; OnPropertyChanged(); } }

        private string _memberPhone;
        public string MemberPhone { get => _memberPhone; set { _memberPhone = value; OnPropertyChanged(); } }

        private string _memberEmail;
        public string MemberEmail { get => _memberEmail; set { _memberEmail = value; OnPropertyChanged(); } }

        private byte[] _imageCover;
        public byte[] ImageCover { get => _imageCover; set { _imageCover = value; OnPropertyChanged(); } }

        private string _updateOrAddContent;
        public string UpdateOrAddContent { get => _updateOrAddContent; set { _updateOrAddContent = value; OnPropertyChanged(); } }


        //Commands
        public ICommand AddImageCommand { get; set; }
        public ICommand AddMemberCommand { get; set; }
        public ICommand DeleteMemberCommand { get; set; }

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
}

