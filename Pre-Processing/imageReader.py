import cv2
img = cv2.imread("14.png",cv2.IMREAD_GRAYSCALE)
img = cv2.bitwise_not(img)

img = cv2.threshold(img, 127, 1, cv2.THRESH_BINARY)[1]


img = img
print(img)
