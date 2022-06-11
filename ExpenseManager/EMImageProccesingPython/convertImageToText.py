from google.cloud import vision
import io
import os


def detect_text_binary(image_bytes) -> str:
    """Detects text in the file."""

    client = vision.ImageAnnotatorClient()

    image = vision.Image(content=image_bytes)

    response = client.text_detection(image=image, image_context={"language_hints": ["ro"]})
    texts = response.text_annotations
    print('Texts:')

    result = ""
    for text in texts:
        result += "\n" + text.description
        break;

    if response.error.message:
        raise Exception(
            '{}\nFor more info on error messages, check: '
            'https://cloud.google.com/apis/design/errors'.format(
                response.error.message))
    return result


def write_in_file_detect_text_result_file_from_bytearray(image_bytes, result_file_path) -> None:
    """Executes detect_text and writes the result inside a txt file"""

    print(detect_text_binary(image_bytes))
    result_string = detect_text_binary(image_bytes)
    f = open(result_file_path, "ab")
    f.write(result_string.encode('utf8'))
    f.close()

    f1 = open(result_file_path, 'r', encoding='utf-8')
    print(f1.read())


def write_in_file_detect_text_result_from_bytearray(image_bytes) -> str:
    """Executes detect_text and writes the result inside a txt file"""

    print(detect_text_binary(image_bytes))
    result_string = detect_text_binary(image_bytes)
    return result_string
