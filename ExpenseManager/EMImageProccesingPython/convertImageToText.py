from google.cloud import vision
import io
import os


MAX = 100000000000

def detect_text_binary(image_bytes) -> str:
    """Detects text in the file."""

    client = vision.ImageAnnotatorClient()
    image = vision.Image(content=image_bytes)

    response = client.text_detection(image=image, image_context={"language_hints": ["ro"]})
    texts = response.text_annotations
    result = ""

    if response.error.message:
        raise Exception(
            '{}\nFor more info on error messages, check: '
            'https://cloud.google.com/apis/design/errors'.format(
                response.error.message))
    else: 
        print('Texts:')

        index = 0
        word_vertices_group_tuple_list = []
        for text in texts:
            result += "\n" + text.description
            if index > 0:
                vert = text.bounding_poly.vertices
                tuple_item = (text.description, vert, 0, calculate_y_coordinate_of_the_center_of_the_rectangle(vert))
                word_vertices_group_tuple_list.append(tuple_item)
            index += 1

        try:
            result = group_words_per_lines(word_vertices_group_tuple_list)
        except Exception as exc:
            print(exc)

    return result


def group_words_per_lines(word_vertices_group_tuple_list) -> str:
    """return words ordered per lines"""
    result = ""
    nb_items = len(word_vertices_group_tuple_list)
    ungrouped_items_nb = nb_items
    group_index = 1

    first_ungrouped_word_in_the_row_index = \
        find_ungrouped_word_index_with_the_lower_y_from_the_next_unprocessed_row(word_vertices_group_tuple_list)
    first_item = word_vertices_group_tuple_list[first_ungrouped_word_in_the_row_index]
    word_vertices_group_tuple_list[first_ungrouped_word_in_the_row_index] = \
        replace_group_in_tuple(group_index, word_vertices_group_tuple_list[first_ungrouped_word_in_the_row_index])
    ungrouped_items_nb -= 1
    while ungrouped_items_nb > 0:
        were_changes = True
        y_min, y_max = find_min_and_max_y(first_item[1])
        while were_changes:
            were_changes = False
            for index1 in range(nb_items):
                if word_vertices_group_tuple_list[index1][-2] == 0 and index1 != first_ungrouped_word_in_the_row_index:
                    item2 = word_vertices_group_tuple_list[index1]
                    if item2[-1] * 2 in range(y_min * 2, y_max * 2):
                        word_vertices_group_tuple_list[index1] = replace_group_in_tuple(group_index, item2)
                        y_min_current, y_max_current = find_min_and_max_y(item2[1])
                        if y_min > y_min_current:
                            y_min = y_min_current
                        if y_max < y_max_current:
                            y_max = y_max_current
                        ungrouped_items_nb -= 1
                        were_changes = True

        group_list = add_tuples_within_the_same_group_to_list(group_index, word_vertices_group_tuple_list) # verificare
        result += rearrange_list_elements_by_coord_x(group_list)+"\n"
        group_index += 1

        first_ungrouped_word_in_the_row_index = \
            find_ungrouped_word_index_with_the_lower_y_from_the_next_unprocessed_row(word_vertices_group_tuple_list)
        first_item = word_vertices_group_tuple_list[first_ungrouped_word_in_the_row_index]
        word_vertices_group_tuple_list[first_ungrouped_word_in_the_row_index] = \
            replace_group_in_tuple(group_index, word_vertices_group_tuple_list[first_ungrouped_word_in_the_row_index])
        ungrouped_items_nb -= 1

    return result


def add_tuples_within_the_same_group_to_list(group_index, word_vertices_group_tuple_list) -> list:
    """group in list tuples with the given group index"""
    nb_items = len(word_vertices_group_tuple_list)
    result_list = []
    for index in range(nb_items):
        if word_vertices_group_tuple_list[index][-2] == group_index:
            result_list.append(word_vertices_group_tuple_list[index])

    return result_list


def rearrange_list_elements_by_coord_x(group_list) -> str:
    """form row giving grouped words"""
    nb_items = len(group_list)
    result_list = []
    for index in range(nb_items):
        tuple1 = (group_list[index], min(group_list[index][1][0].x, group_list[index][1][-1].x))
        result_list.append(tuple1)

    result_line = ""
    words_added_nb = 0
    x_limit = 0
    min_index = 0
    while words_added_nb < nb_items and min_index > -1:
        min_x = MAX
        min_index = -1
        for index in range(0, nb_items):
            if x_limit <= result_list[index][1]:
                if min_x == MAX:
                    min_x = result_list[index][1]
                    min_index = index
                else:
                    if min_x > result_list[index][1]:
                        min_x = result_list[index][1]
                        min_index = index

        if min_index > -1:
            result_line += result_list[min_index][0][0] + " "
            x_limit = max(result_list[min_index][0][1][1].x, result_list[min_index][0][1][-2].x)
            words_added_nb += 1

    return result_line


def replace_group_in_tuple(new_group_no, my_tuple) -> tuple:
    """replace group_index inside a tuple"""
    aux = list(my_tuple)
    aux[-2] = new_group_no
    return tuple(aux)


def calculate_y_coordinate_of_the_center_of_the_rectangle(vertices) -> float:
    """calculates the y coordinate of the center of the rectangle the word is contained"""
    return (vertices[0].y + vertices[2].y) / 2.0


def find_ungrouped_word_index_with_the_lower_y_from_the_next_unprocessed_row(word_vertices_group_tuple_list) -> int:
    """Finds the minimium y coordinate of the next unprocessed row"""
    nb_items = len(word_vertices_group_tuple_list)
    min_y = MAX
    min_index = -1
    for index in range(0, nb_items):
        if word_vertices_group_tuple_list[index][-2] == 0:
            if min_y > word_vertices_group_tuple_list[index][1][0].y:
                min_y = word_vertices_group_tuple_list[index][1][0].y
                min_index = index

    return min_index


def find_min_and_max_y(vertices) -> tuple:
    """returns y_min and y_max between which a row is contained"""
    y_min = vertices[0].y
    y_max = vertices[0].y
    for index in range(1, 4):
        if y_min > vertices[index].y:
            y_min = vertices[index].y
        if y_max < vertices[index].y:
            y_max = vertices[index].y
    return y_min, y_max


def write_in_file_detect_text_result_from_bytearray(image_bytes) -> str:
    """Executes detect_text and returns the result as string"""

    result_string = detect_text_binary(image_bytes)
    print(result_string)
    return result_string


def write_in_file_detect_text_result_file_from_bytearray(image_bytes, result_file_path) -> None:
    """Executes detect_text and writes the result inside a txt file"""

    result_string = detect_text_binary(image_bytes)
    print(result_string)
    f = open(result_file_path, "ab")
    f.write(result_string.encode('utf8'))
    f.close()

    f1 = open(result_file_path, 'r', encoding='utf-8')
    print(f1.read())
