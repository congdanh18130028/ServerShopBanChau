using AutoMapper;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Shop.Dtos;
using Shop.Models;
using Shop.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private static String apiKey = "AIzaSyCz_xGQ0v5C5Kvr3oY6RMTa-l_bvbPBdFI";
        private static String Bucket = "upload-123f2.appspot.com";
        private static String AuthEmail = "congdanh04092000@gmail.com";
        private static String AuthPassword = "danh123###";

        private readonly IProductsServices _productsServices;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _env;


        public ProductsController(IProductsServices productsServices, IMapper mapper, IHostingEnvironment env)
        {
            _productsServices = productsServices;
            _mapper = mapper;
            _env = env;
        }

        [HttpGet]
        public ActionResult <IEnumerable<ProductReadDto>> GetProducts()
        {
            var products = _productsServices.GetProducts();
            return Ok(_mapper.Map<IEnumerable<ProductReadDto>>(products));
        }

        [HttpGet("search")]
        public ActionResult<IEnumerable<ProductReadDto>> GetProductsSearch(String value)
        {
            var products = _productsServices.GetProductsSearch(value);
            return Ok(_mapper.Map<IEnumerable<ProductReadDto>>(products));
        }

        [HttpGet("desc")]
        public ActionResult<IEnumerable<ProductReadDto>> GetProductsDescPrice()
        {
            var products = _productsServices.GetProductsDescPrice();
            return Ok(_mapper.Map<IEnumerable<ProductReadDto>>(products));
        }

        [HttpGet("category/desc")]
        public ActionResult<IEnumerable<ProductReadDto>> GetProductsCategoryDescPrice(String category)
        {
            var products = _productsServices.GetProductsCategoryDescPrice(category);
            return Ok(_mapper.Map<IEnumerable<ProductReadDto>>(products));
        }



        [HttpGet("asc")]
        public ActionResult<IEnumerable<ProductReadDto>> GetProductsAscPrice()
        {
            var products = _productsServices.GetProductsAscPrice();
            return Ok(_mapper.Map<IEnumerable<ProductReadDto>>(products));
        }

        [HttpGet("category/asc")]
        public ActionResult<IEnumerable<ProductReadDto>> GetProductsCategoryAscPrice(String category)
        {
            var products = _productsServices.GetProductsCategoryAscPrice(category);
            return Ok(_mapper.Map<IEnumerable<ProductReadDto>>(products));
        }

        [HttpGet("category")]
        public ActionResult<IEnumerable<ProductReadDto>> GetProductsByCategory(String category)
        {
            var products = _productsServices.GetProductsByCategory(category);
            return Ok(_mapper.Map<IEnumerable<ProductReadDto>>(products));
        }

        [HttpPost("category")]
        public ActionResult AddCategory([FromForm]String categoryName)
        {
            var category = new Category(categoryName);
            _productsServices.AddCaterory(category);
            _productsServices.SaveChanges();
            return Ok();
        }

        [HttpGet("categories")]
        public ActionResult<IEnumerable<Category>> GetCategories()
        {
            var categories = _productsServices.GetCategories();
            return Ok(categories);
        }

        [HttpGet("price")]
        public ActionResult<IEnumerable<ProductReadDto>> GetProductsByPrice(int price1, int price2)
        {
            var products = _productsServices.GetProductsByPrice(price1, price2);
            return Ok(_mapper.Map<IEnumerable<ProductReadDto>>(products));
        }

        [HttpGet("category/price")]
        public ActionResult<IEnumerable<ProductReadDto>> GetProductsCategoryByPrice(String category, int price1, int price2)
        {
            var products = _productsServices.GetProductsCategoryByPrice(category, price1, price2);
            return Ok(_mapper.Map<IEnumerable<ProductReadDto>>(products));
        }


        [HttpGet("{id}", Name ="GetProduct")]
        public ActionResult GetProduct(int id)
        {
            var product = _productsServices.GetProduct(id);
            if(product != null)
            {
                return Ok(_mapper.Map<ProductReadDto>(product));
            }
            return NotFound($"Product with id: {id} was not found");
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct([FromForm]String name, [FromForm]String category, [FromForm]FileUpload file, [FromForm]String description, [FromForm]int quantity, [FromForm]int price)
        {
            //upload file to firebase;

            var _name = name;
            var _category = category;
            var _link = "";
            var _description = description;
            var _quantity = quantity;
            var _price = price;

            var fileUpload = file.File;
            FileStream fs = null;
            if (fileUpload.Length > 0)
            {
                String foldername = "firebaseFiles";
                String path = Path.Combine(_env.WebRootPath, $"images/{foldername}");
                if (Directory.Exists(path))
                {
                    using (fs = new FileStream(Path.Combine(path, fileUpload.FileName), FileMode.Create))
                    {
                        await fileUpload.CopyToAsync(fs);
                    }
                    fs = new FileStream(Path.Combine(path, fileUpload.FileName), FileMode.Open);

                }
                else
                {
                    Directory.CreateDirectory(path);
                }


                var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
                var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

                var cancellation = new CancellationTokenSource();
                var upload = new FirebaseStorage(
                    Bucket,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true
                    })
                    .Child("assets")
                    .Child($"{fileUpload.FileName}.{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}")
                    .PutAsync(fs, cancellation.Token);
                try
                {
                    _link = await upload;
                    var filePath = new FilePath(_link);
                    List<FilePath> listFilePath = new List<FilePath>();
                    listFilePath.Add(filePath);

                    var product = new ProductCreateDto(name, category, listFilePath, description, quantity, price);
                    var _product = _mapper.Map<Product>(product);
                    _productsServices.AddProduct(_product);
                    _productsServices.SaveChanges();
                    var productReadDto = _mapper.Map<ProductReadDto>(_product);
                    return CreatedAtRoute(nameof(GetProduct), new { id = productReadDto.Id }, productReadDto);
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
            return BadRequest();
        }

        [HttpPatch("{id}")]
        public ActionResult UpdateProduct(int id, [FromBody] JsonPatchDocument<ProductUpdateDto> patch)
        {
            var _product = _productsServices.GetProduct(id);
            if(_product == null)
            {
                return NotFound();
            }
            var product = _mapper.Map<ProductUpdateDto>(_product);
            patch.ApplyTo(product, ModelState);
            if (!TryValidateModel(product))
            {
                return ValidationProblem();
            }
            _mapper.Map(product, _product);
            _productsServices.UppdateProduct(_product);
            _productsServices.SaveChanges();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public ActionResult DeleteProduct(int id)
        {
            var _product = _productsServices.GetProduct(id);
            if (_product == null)
            {
                return NotFound($"Product with id: {id} was not found");
            }
            _productsServices.DeleteProduct(_product);
            _productsServices.SaveChanges();
            return NoContent();
        }
    }
}
