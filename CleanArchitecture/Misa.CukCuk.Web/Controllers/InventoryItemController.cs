using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Misa.Core.Entities;
using Misa.Core.Entities.Category;
using Misa.Core.Entities.DataController;
using Misa.Core.Entities.Page;
using Misa.Core.Enum;
using Misa.Core.Interfaces.Repository;
using Misa.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Misa.CukCuk.Web.Controllers
{
    [Route("api/v1/[controller]s")]
    [ApiController]
    public class InventoryItemController : ControllerBase
    {
        #region DECLARE
        IInventoryItemRepository _inventoryItemRepository;
        IInventoryItemService _inventoryItemServices;
        public static IWebHostEnvironment _environment;
        #endregion

        #region CONSTRUCTOR
        public InventoryItemController(IInventoryItemRepository inventoryItemRepository, IInventoryItemService inventoryItemServices, IWebHostEnvironment environment)
        {
            _inventoryItemRepository = inventoryItemRepository;
            _inventoryItemServices = inventoryItemServices; 
            _environment = environment;
        }
        #endregion

        #region METHOD

        #region GET
        /// <summary>
        /// Trả về danh sách hàng hóa theo phân trang, tìm kiếm, sắp xếp
        /// </summary>
        /// <param name="page">Dữ liệu chứa đầy đủ các thông tin phân trang tìm kiếm, sắp xếp</param>
        /// <returns>Danh sách hàng hóa</returns>
        /// Created By: LMCUONG(12/07/2021)
        [HttpPost]
        public async Task<IActionResult> GetOptiopPage([FromBody] OptionPage page)
        {

            ActionServiceResult result = await _inventoryItemServices.GetOptionPage(page);

            if (result.Success == true)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }


        }

        [HttpGet("SKUCodeMax")]
        public async Task<IActionResult> GetSKUCodeMax()
        {
            string SKUCodeMax = await _inventoryItemServices.GetCodeMax(Properties.Resources.InventoryItemTable, Properties.Resources.SKUCode);
            if (string.IsNullOrEmpty(SKUCodeMax))
            {
                return BadRequest(new ActionServiceResult()
                {
                    Data = SKUCodeMax,
                    Success = false,
                    Code = Core.Enum.MISACode.Validate,
                    Messenge = "Lấy mã thành công!"
                });
            }
            else
            {
                return Ok(new ActionServiceResult()
                {
                    Data = SKUCodeMax,
                    Success = true,
                    Code = Core.Enum.MISACode.Success
                });
            }
        }
        [HttpGet("BarCodeMax")]
        public async Task<IActionResult> GetBarCodeMax()
        {
            string SKUCodeMax = await _inventoryItemServices.GetCodeMax(Properties.Resources.InventoryItemTable, Properties.Resources.BarCode);
            if (string.IsNullOrEmpty(SKUCodeMax))
            {
                return BadRequest(new ActionServiceResult()
                {
                    Data = SKUCodeMax,
                    Success = false,
                    Code = Core.Enum.MISACode.Validate,
                    Messenge = "Lấy mã thành công!"
                });
            }
            else
            {
                return Ok(new ActionServiceResult()
                {
                    Data = SKUCodeMax,
                    Success = true,
                    Code = Core.Enum.MISACode.Success
                });
            }
        }



        /// <summary>
        /// Lấy thông tin Hàng hóa theo ID
        /// </summary>
        /// <param name="Id">Mã hàng hóa</param>
        /// <param name="type">Loại hàng hóa: 1 - Hàng hóa thường, 2 - Combo, 3 - Dịch vụ</param>
        /// <returns>Đối tượng hàng hóa chi tiết</returns>
        /// Created By: LMCUONG(16/07/2021)
        [HttpPost("{Id}")]
        public async Task<IActionResult> GetInventoryItemById(Guid Id, ItemType type)
        {
            DetailItem result = new DetailItem();
            switch (type)
            {
                case ItemType.Merchandise:
                    result = await _inventoryItemServices.GetMerchandiseByID(Id);
                    break;
                case ItemType.Combo:
                    result = await _inventoryItemServices.GetComboByID(Id);
                    break;
                case ItemType.Service:
                    result = await _inventoryItemServices.GetMerchandiseByID(Id);
                    break;
                default:
                    break;
            }

            if (result.inventoryItem != null)
            {
                return Ok(new ActionServiceResult()
                {
                    Success = true,
                    Code = MISACode.Success,
                    Data = result,
                    Messenge = "Lấy dữ liệu thành công"
                });
            }
            else
            {
                return BadRequest(new ActionServiceResult()
                {
                    Success = false,
                    Code = MISACode.Exception,
                    Data = result,
                    Messenge = "Lấy dữ liệu không thành công!"
                });
            }

        }
        #endregion

        #region INSERT
        /// <summary>
        /// Thêm danh sách hàng hóa mới
        /// </summary>
        /// <param name="detailItem">Danh sách hàng hóa nhận nào</param>
        /// <returns>Thành công hay thất bại</returns>
        /// Created By: LMCUONG(16/07/2021)
        [HttpPost("InsertMerchandise")]
        public async Task<IActionResult> InsertInventoryItem([FromBody] DetailItem detailItem)
        {
            detailItem.inventoryItem = HandlePicture(detailItem.inventoryItem);

            int rowAffect = await _inventoryItemServices.InsertMerchandises(detailItem);

            return Ok(new ActionServiceResult() {
                Success = true,
                Data = new
                {
                    rowAffect = rowAffect
                },
                Code = MISACode.Success,
                Messenge = "Thêm mới hàng hóa hoặc dịch vụ thành công!"
            });
        }
        [HttpPost("InsertCombo")]
        public async Task<IActionResult> InsertCombo([FromBody] DetailItem detailItem)
        {
            // Xử lý ảnh
            detailItem.inventoryItem = HandlePicture(detailItem.inventoryItem);

            int rowAffect = await _inventoryItemServices.InsertCombos(detailItem);

            return Ok(new ActionServiceResult()
            {
                Success = true,
                Data = new
                {
                    rowAffect = rowAffect
                },
                Code = MISACode.Success,
                Messenge = "Thêm mới Combo thành công!"
            });
        }
        #endregion

        #region UPDATE
        /// <summary>
        /// Sửa danh sách hóa đơn 
        /// </summary>
        /// <param name="detailItem">Danh sách hàng hóa nhận nào</param>
        /// <returns>Thành công hay thất bại</returns>
        /// Created By: LMCUONG(16/07/2021)
        [HttpPut("UpdateMerchandise")] 
        public async Task<IActionResult> UpdateInventoryItem([FromBody] DetailItem detailItem)
        {
            // Xử lý ảnh
            detailItem.inventoryItem = HandlePicture(detailItem.inventoryItem);

            int rowAffect = await _inventoryItemServices.UpdateMerchandises(detailItem);

            return Ok(new ActionServiceResult()
            {
                Success = true,
                Data = new
                {
                    rowAffect = rowAffect
                },
                Code = MISACode.Success,
                Messenge = "Thêm mới thành công!"
            });
        }

        [HttpPut("UpdateCombo")]
        public async Task<IActionResult> UpdateCombo([FromBody] DetailItem detailItem)
        {
            detailItem.inventoryItem = HandlePicture(detailItem.inventoryItem);

            int rowAffect = await _inventoryItemServices.UpdateCombos(detailItem);

            return Ok(new ActionServiceResult()
            {
                Success = true,
                Data = new
                {
                    rowAffect = rowAffect
                },
                Code = MISACode.Success,
                Messenge = "Thêm mới thành công!"
            });
        }

        [HttpPost("HandlePicture")]
        public InventoryItem HandlePicture(InventoryItem inventoryItem)
        {
            try
            {
                // Nếu không có folder tạo folder
                if (!Directory.Exists(_environment.WebRootPath + "\\InventoryItem\\"))
                {
                    Directory.CreateDirectory(_environment.WebRootPath + "\\InventoryItem\\");
                }
                if (!string.IsNullOrWhiteSpace(inventoryItem.PictureBase64))
                {

                    // Tạo đường dẫn từ file mới tải
                    byte[] bytes = Convert.FromBase64String(inventoryItem.PictureBase64);

                    //Tạo kiểu có ảnh
                    System.Drawing.Imaging.ImageFormat imageFormat = System.Drawing.Imaging.ImageFormat.Png;

                    string[] pictureType = inventoryItem.PictureType.Split("/");

                    if (pictureType[0].Equals("image"))
                    {
                        // Gán kiểu cho ảnh
                        if (pictureType[1].Equals("jpg") || pictureType[1].Equals("jpeg"))
                        {
                            imageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
                        }
                        else if (pictureType[1].Equals("tiff"))
                        {
                            imageFormat = System.Drawing.Imaging.ImageFormat.Tiff;
                        }
                        else if (pictureType[1].Equals("gif"))
                        {
                            imageFormat = System.Drawing.Imaging.ImageFormat.Gif;

                        }
                        else if (pictureType[1].Equals("bmp"))
                        {
                            imageFormat = System.Drawing.Imaging.ImageFormat.Bmp;
                        }


                        // Nếu đã có đường dẫn file, xóa file
                        if (!string.IsNullOrEmpty(inventoryItem.PictureLocation))
                        {
                            FileInfo filea = new FileInfo(_environment.WebRootPath + inventoryItem.PictureLocation);

                            if (filea.Length > 0)
                            {
                                filea.Delete();
                            }
                        }

                        // Tạo file ảnh
                        inventoryItem.PictureID = Guid.NewGuid();
                        System.Drawing.Image image;
                        using (MemoryStream ms = new MemoryStream(bytes))
                        {
                            image = System.Drawing.Image.FromStream(ms);
                        }

                        image.Save(_environment.WebRootPath + "\\InventoryItem\\" + inventoryItem.PictureID + inventoryItem.PictureName, imageFormat);
                        inventoryItem.PictureLocation = "\\InventoryItem\\" + inventoryItem.PictureID + inventoryItem.PictureName;
                    }

                }
            }
            catch (Exception)
            {

                return inventoryItem;
            }
            return inventoryItem;
        }
        #endregion

        #region DELETE
        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteInventoryItemByListID([FromBody] List<Guid> listID)
        {
            int rowAffect = await _inventoryItemRepository.DeleteInventoryItemByListID(listID);

            return Ok(new ActionServiceResult()
            {
                Success = true,
                Data = new
                {
                    rowAffect = rowAffect
                },
                Code = MISACode.Success,
                Messenge = "Xóa thành công!"
            });
        }
        #endregion

        #endregion
    }
}
