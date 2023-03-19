using NTech.Base.Commons.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTech.Xm.Database.Models
{
    public class MessagesDetailModel : BaseModel, ICloneable
    {
        //public MessagesDetailModel()
        //{

        //}
        //public MessagesDetailModel(MessagesDetailModel messagesDetailModel)
        //{

        //}
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        private string _manufacturingDate;
        private string _manufacturingTime;
        private string _manufacturingShift;
        private string _couponCode;
        private string _licensePlate;
        private string _customerCode;
        private string _customerName;
        private string _cementType;
        private float _weightTon;
        private int _numberBags;
        private int _numberBagsPrinted;
        private string _bagCoverType;
        private string _brand;
        private string _arrangeType;
        private string _line;
        private string _printer;
        private string _trough;
        private string _messageState;
        private string _note;

        public Guid Guid { get; set; }
        public string ManufacturingDate { get { return _manufacturingDate; } set { SetProperty(ref _manufacturingDate, value); } } /*ngày sản xuất - dd/MM/YYYY*/
        public string ManufacturingTime { get { return _manufacturingTime; } set { SetProperty(ref _manufacturingTime, value); } } /*thời gian sản xuất - HH:mm:ss*/
        public string ManufacturingShift { get { return _manufacturingShift; } set { SetProperty(ref _manufacturingShift, value); } } /*ca sản xuất*/
        public string CouponCode { get { return _couponCode; } set { SetProperty(ref _couponCode, value); } } /*số phiếu*/
        public string LicensePlate { get { return _licensePlate; } set { SetProperty(ref _licensePlate, value); } } /*biển số xe*/
        public string CustomerCode { get { return _customerCode; } set { SetProperty(ref _customerCode, value); } } /*mã khách hàng*/
        public string CustomerName { get { return _customerName; } set { SetProperty(ref _customerName, value); } } /*tên khách hàng*/
        public string CementType { get { return _cementType; } set { SetProperty(ref _cementType, value); } } /*loại xi măng*/
        public float WeightTon { get { return _weightTon; } set { SetProperty(ref _weightTon, value); } } /*trọng lượng, tính theo đơn vị tấn*/
        public int NumberBags { get { return _numberBags; } set { SetProperty(ref _numberBags, value); } } /*số bao cần in*/
        public int NumberBagsPrinted { get { return _numberBagsPrinted; } set { SetProperty(ref _numberBagsPrinted, value); } } /*số bao đã in*/
        public string BagCoverType { get { return _bagCoverType; } set { SetProperty(ref _bagCoverType, value); } } /*loại vỏ bao: KPK, KP, PP*/
        public string Brand { get { return _brand; } set { SetProperty(ref _brand, value); } } /*thương hiệu xi măng: Thành Thắng, Thịnh Thành*/
        public string ArrangeType { get { return _arrangeType; } set { SetProperty(ref _arrangeType, value); } } /*kiểu xếp hàng: dải dây, Pallet, xe công*/
        public string Line { get { return _line; } set { SetProperty(ref _line, value); } } /*Id Line: 1, 2, 3*/
        public string Printer { get { return _printer; } set { SetProperty(ref _printer, value); } } /*Id Máy in*/
        public string Trough { get { return _trough; } set { SetProperty(ref _trough, value); } } /*Id Máng xuất*/
        public string MessageState { get { return _messageState; } set { SetProperty(ref _messageState, value); } } /*Trạng thái của bản tin: Chưa in, Chưa hoàn thành, Đã hoàn thành*/
        public string Note { get { return _note; } set { SetProperty(ref _note, value); } }
    }
}
