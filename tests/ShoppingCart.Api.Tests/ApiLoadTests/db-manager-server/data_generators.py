import random

TITLES = [
    'Apple', 
    'Watermelon', 
    'Apricot', 
    'Banana', 
    'Blackberry', 
    'Blueberry',
    'Boysenberry', 
    'Canary Melon', 
    'Pear', 
    'Orange' 
]
IMAGES = [
    "https://tailwindui.com/img/ecommerce-images/shopping-cart-page-04-product-01.jpg",
    "https://tailwindui.com/img/ecommerce-images/shopping-cart-page-04-product-02.jpg",
]
    
def random_title():
    index = random.randint(0, len(TITLES) - 1)
    return TITLES[index]
    
        
def random_image():
    index = random.randint(0, len(IMAGES) - 1)
    return IMAGES[index]