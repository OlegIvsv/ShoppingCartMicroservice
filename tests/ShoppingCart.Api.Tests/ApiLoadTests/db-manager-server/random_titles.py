import random

TITLES = ['Apple', 'Watermelon', 'Apricot', 'Banana', 'Blackberry', 'Blueberry',
'Boysenberry', 'Canary Melon', 'Cantaloupe', 'Casaba Melon', 'Pear', 'Orange' ]
    
def random_title():
    index = random.randint(0, len(TITLES) - 1)
    return TITLES[index]