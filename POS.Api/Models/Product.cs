// So understand this : Our product class is representing a kind of Product.

//this code below is creating filepath for this Product.cs file
//so if another file does using POS.Api.Models they will be able to access this Product.cs file.
namespace POS.Api.Models

{
    public class Product
    {
        public int Id{get; set;}
        public string Name{get;set;}=string.Empty;
        //so why is this LOC different from the above.
        //So, in dotnet there are sometimes error showing due to null values.
        //for example if by mistake Name is left Null sometimes -- it shows null in its value and it causes error in
        //the system
        //But int are 0 by default and are not null.
        public decimal Price{get;set;}
     }
}