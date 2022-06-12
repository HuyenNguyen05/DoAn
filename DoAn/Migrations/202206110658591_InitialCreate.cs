namespace BTL_Nhom13.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChiTietGioHang",
                c => new
                    {
                        MaGioHang = c.Int(nullable: false),
                        MaSP = c.Int(nullable: false),
                        SoLuongMua = c.Int(nullable: false),
                        Gia = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.MaGioHang, t.MaSP })
                .ForeignKey("dbo.GioHang", t => t.MaGioHang)
                .ForeignKey("dbo.SanPham", t => t.MaSP)
                .Index(t => t.MaGioHang)
                .Index(t => t.MaSP);
            
            CreateTable(
                "dbo.GioHang",
                c => new
                    {
                        MaGioHang = c.Int(nullable: false, identity: true),
                        TenTaiKhoan = c.String(nullable: false, maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.MaGioHang)
                .ForeignKey("dbo.TaiKhoan", t => t.TenTaiKhoan)
                .Index(t => t.TenTaiKhoan);
            
            CreateTable(
                "dbo.HoaDon",
                c => new
                    {
                        MaHoaDon = c.Int(nullable: false, identity: true),
                        NgayDat = c.DateTime(nullable: false),
                        TinhTrang = c.String(nullable: false, maxLength: 100),
                        PhiShip = c.Decimal(nullable: false, storeType: "money"),
                        GhiChu = c.String(storeType: "ntext"),
                        DcNhanHang = c.String(storeType: "ntext"),
                        MaGioHang = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MaHoaDon)
                .ForeignKey("dbo.GioHang", t => t.MaGioHang)
                .Index(t => t.MaGioHang);
            
            CreateTable(
                "dbo.TaiKhoan",
                c => new
                    {
                        TenTaiKhoan = c.String(nullable: false, maxLength: 20, unicode: false),
                        MatKhau = c.String(nullable: false, maxLength: 20, unicode: false),
                        Quyen = c.Int(nullable: false),
                        TinhTrang = c.Boolean(nullable: false),
                        TenKhachHang = c.String(nullable: false, maxLength: 30),
                        Email = c.String(nullable: false, maxLength: 100),
                        SoDienThoai = c.String(nullable: false, maxLength: 12, unicode: false),
                        DiaChi = c.String(nullable: false, storeType: "ntext"),
                    })
                .PrimaryKey(t => t.TenTaiKhoan);
            
            CreateTable(
                "dbo.SanPham",
                c => new
                    {
                        MaSP = c.Int(nullable: false, identity: true),
                        MaDM = c.Int(nullable: false),
                        TenSP = c.String(nullable: false, maxLength: 100),
                        NhaSX = c.String(nullable: false, maxLength: 100),
                        TrongLuong = c.Double(nullable: false),
                        SoLuongTon = c.Int(nullable: false),
                        Gia = c.Decimal(nullable: false, precision: 18, scale: 0),
                        ChatLuong = c.String(nullable: false, maxLength: 100),
                        MoTa = c.String(storeType: "ntext"),
                        Anh = c.String(),
                    })
                .PrimaryKey(t => t.MaSP)
                .ForeignKey("dbo.DanhMuc", t => t.MaDM)
                .Index(t => t.MaDM);
            
            CreateTable(
                "dbo.DanhMuc",
                c => new
                    {
                        MaDM = c.Int(nullable: false, identity: true),
                        TenDM = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.MaDM);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SanPham", "MaDM", "dbo.DanhMuc");
            DropForeignKey("dbo.ChiTietGioHang", "MaSP", "dbo.SanPham");
            DropForeignKey("dbo.GioHang", "TenTaiKhoan", "dbo.TaiKhoan");
            DropForeignKey("dbo.HoaDon", "MaGioHang", "dbo.GioHang");
            DropForeignKey("dbo.ChiTietGioHang", "MaGioHang", "dbo.GioHang");
            DropIndex("dbo.SanPham", new[] { "MaDM" });
            DropIndex("dbo.HoaDon", new[] { "MaGioHang" });
            DropIndex("dbo.GioHang", new[] { "TenTaiKhoan" });
            DropIndex("dbo.ChiTietGioHang", new[] { "MaSP" });
            DropIndex("dbo.ChiTietGioHang", new[] { "MaGioHang" });
            DropTable("dbo.DanhMuc");
            DropTable("dbo.SanPham");
            DropTable("dbo.TaiKhoan");
            DropTable("dbo.HoaDon");
            DropTable("dbo.GioHang");
            DropTable("dbo.ChiTietGioHang");
        }
    }
}
