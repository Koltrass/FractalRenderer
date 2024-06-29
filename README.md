# UltrabasicFractalRenderer
Program for rendering some 2D fractals.

Project was done for a computer graphics course and then improved a bit out of interest.

## Instruction
Program allows rendering following fractals:
- Multibrot (and Burning Ship, accessed in same tab as Multibrot);
- Multijulia (and sin(z^p)*c);
- IFS fractals;

For each fractal type, different parameters can be set.

![Screenshot_1](https://github.com/Koltrass/UltrabasicFractalRenderer/assets/174179522/9bf18fcc-2e56-4cff-b0c8-e1b48f7aeba1)

Control elements from top to bottom, left to right:
- display part;
- +/- for increasing/decreasing size of unit by multiplying/dividing by set value;
- button for saving displayed image as file;
- several tabs for choosing fractal type and setting their parameters;
- unit size (in pixels);
- real coordinates of center;
- button for drawing fractal;
- render time.

Image can be centered around any point by left-clicking it.

To save fractal resized to your screen size, press alt+f. This may take significantly more time to render. 
## Explanation for tabs and parameters

### Multibrot tab
Allows rendering Multibrot and Burning Ship fractals.

![Screenshot_2](https://github.com/Koltrass/UltrabasicFractalRenderer/assets/174179522/0c9a9b7e-ed7f-4110-a445-581cf3e71b36)

- Max n of iterations – max amount of iterations before color of each pixel is determined. The bigger it is, the more detailed picture will be recieved (in some cases image may become too dark - decreasing this value should brighten it).  Should be gradually increased when zooming in.
- Z exponent – power of Z in iterated function;
- f(z, c)=? – selector of formula to be iterated. z^p+c is for Multibrot, abs(z^p)+c for Burning Ship fractal;
- Color scheme – selector of fractal coloring;
- Escape r^2 – determines radius of a circle. If iterated point goes beyond it, iteration process for a point is stopped.
### Multijulia tab
Allows rendering Multijulia and sin(z) fractals.

![Screenshot_3](https://github.com/Koltrass/UltrabasicFractalRenderer/assets/174179522/413d4654-b8a9-4685-8cc4-31045ac68cd9)

- C real – real part of C constant for iterations;
- C imaginary – imaginary part of C constant.
- f(z, c)=? – selector of formula to be iterated. First option renders Multijulia fractals.
- others are same as in Multibrot tab.
### IFS tab
Allows to render any fractal which can be described by Iterated Function System.

![Screenshot_4](https://github.com/Koltrass/UltrabasicFractalRenderer/assets/174179522/8790f746-0fee-41d1-bc0c-569cb4907b2b)

- Each row in table corresponds to one affine transformation. a, b, c, d, e, f, g are coefficients of transormation, p is probability. C is color for each transformation.
- Amount of iterations determines how many iterations will happen.
## Gallery
![2024-3-20-15-8-36-1](https://github.com/Koltrass/UltrabasicFractalRenderer/assets/174179522/e9d3c0ff-6ff2-4afa-af61-2b98acfcf10c)
![2024-3-20-15-10-33-1](https://github.com/Koltrass/UltrabasicFractalRenderer/assets/174179522/7a415117-bc8d-4773-acaf-c8803b05ff2f)
![2024-3-20-15-31-58-1](https://github.com/Koltrass/UltrabasicFractalRenderer/assets/174179522/6dcba8fd-ef26-4f45-ad06-49780fdb2b76)
![2024-3-20-15-42-31](https://github.com/Koltrass/UltrabasicFractalRenderer/assets/174179522/efcbb0a4-a5db-43a9-b617-d9067bf0a32f)
![2024-3-21-19-56-50-1](https://github.com/Koltrass/UltrabasicFractalRenderer/assets/174179522/8893707e-a6a8-492b-a054-73f72835008b)
![2024-3-21-21-7-10-1](https://github.com/Koltrass/UltrabasicFractalRenderer/assets/174179522/d24e7707-f863-4e4c-8813-ecda3ce0cd20)
![2024-3-21-21-20-56-1](https://github.com/Koltrass/UltrabasicFractalRenderer/assets/174179522/fbd63257-08c4-43d1-a58b-bc158e78cfda)
![2024-3-22-17-46-29-1](https://github.com/Koltrass/UltrabasicFractalRenderer/assets/174179522/046c2edf-ac4e-4a68-84b3-86ef998cd78c)
