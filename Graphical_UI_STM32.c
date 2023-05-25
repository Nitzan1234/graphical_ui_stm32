#include "stm32f401xc.h"
#include "stdio.h"
#include "core_cm4.h"

// Function to send a character to the USB CDC (VCP)
int sendCharacter(int ch)
{
    while (!(USART2->SR & USART_SR_TXE)); // Wait for TX buffer to be empty
    USART2->DR = ch; // Send character
    return ch;
}

// Function to redirect stdout to USART2
int __io_putchar(int ch)
{
    if (ch == '\n')
    {
        sendCharacter('\r'); // Send carriage return before newline character
    }
    return sendCharacter(ch);
}

int main(void)
{
    // Enable USART2 clock
    RCC->APB1ENR |= RCC_APB1ENR_USART2EN;

    // Configure USART2 pins (PA2: USART2_TX, PA3: USART2_RX)
    RCC->AHB1ENR |= RCC_AHB1ENR_GPIOAEN;
    GPIOA->AFR[0] |= 0x700; // Set alternate function AF7 for PA2 and PA3
    GPIOA->MODER |= GPIO_MODER_MODE2_1 | GPIO_MODER_MODE3_1; // Set PA2 and PA3 to alternate function mode
    GPIOA->OTYPER &= ~(GPIO_OTYPER_OT2 | GPIO_OTYPER_OT3); // Set PA2 and PA3 as push-pull
    GPIOA->OSPEEDR |= GPIO_OSPEEDER_OSPEED2_1 | GPIO_OSPEEDER_OSPEED3_1; // Set high speed for PA2 and PA3

    // Configure USART2
    USART2->BRR = 0x0683; // Set baud rate to 115200 (assuming the system clock is 16 MHz)
    USART2->CR1 = USART_CR1_UE | USART_CR1_TE; // Enable USART2 and transmitter


        // Your main program logic here
        printf("54+");
        printf("38*"); // Example print statement
    }
}
