using ErrorOr;

namespace Application.Services.Image;

public static class ImageErrors
{
    public static Error CannotUpload => Error.Failure(
        code: "Image.CannotUpload",
        description: "There was an error during image upload");

    public static Error CannotDelete => Error.Failure(
        code: "Image.CannotDelete",
        description: "There was an error during image deletion");
}
