import json
from convertImageToText import write_in_file_detect_text_result_from_bytearray
from flask import Flask, jsonify, request

HOSTNAME = "192.168.43.186" # se va inlocui cu adresa locala de internet la care sunt conectate 
                            # laptopul/desktop-ul si device-ul mobil pe care este rulat programul
PORT = 9000
JSON_PATH = "./JsonFiles/ProductDictionaryJsonFile.json"

app = Flask(__name__)

@app.route('/', methods=['POST'])
def get_text_from_image():
    """Extracts text from image and writes it into text file"""

    data = json.loads(request.data)
    data1 = json.loads(data)
    image_bytes = data1.get('imageBytes')
    print("type(image_bytes):", type(image_bytes))
    output_string = write_in_file_detect_text_result_from_bytearray(image_bytes)
    
    return output_string


@app.route('/json', methods=['GET', 'POST'])
def working_with_json():
    """Returns the json that contains Product Dictionary wrapped as Response"""
    if request.method == 'GET':
        dictionary = import_dictionary_from_json_file(JSON_PATH)
        return json.dumps(dictionary)

    data_list = json.loads(request.data)
    update_dictionary_in_json_file(JSON_PATH, data_list)
    return "Json file has been updated"


def import_dictionary_from_json_file(json_filepath):
    """Extracts json data and converts it into a dictionary"""
    file_object = open(json_filepath)
    json_content = file_object.read()
    return json.loads(json_content)

def update_dictionary_in_json_file(json_filepath, data_list):
    """Serializes data_list and saves the information into a json file"""  
    list_string = json.dumps(data_list)
    dictionary = json.loads(json.loads(list_string))
    with open(json_filepath, "w") as file:
        json.dump(dictionary,file)
    

def flask_run():
    app.run(host=HOSTNAME, port=PORT)
