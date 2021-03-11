using Calendar.Models.Attributes;

namespace Calendar.Models.Result
{
    public enum ResponseMessageType
    {
        [HttpStatus(200)] None,
        [HttpStatus(201)] EmptyResult,
        [HttpStatus(500)] InternalError,
        [HttpStatus(400)] IdIsMissing,
        [HttpStatus(404)] NotFound,
        [HttpStatus(400)] InvalidModel,
        [HttpStatus(400)] InvalidId
    }
}