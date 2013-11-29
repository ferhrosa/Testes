#include <iostream>

int main()
{
    int number = 10;
    int *pnumber = &number;

    std::cout
        << "number: " << number << std::endl
        << "&number: " << &number << std::endl
        << "&pnumber: " << &pnumber << std::endl
        << "pnumber: " << pnumber << std::endl
        << "*pnumber: " << *pnumber << std::endl;


    char *nome = "Fernando Rosa";
    int i = 0;

    while (nome[i] != '\0')
        std::cout << nome[i++] << std::endl;

    
    char *nome2 = "Fernando 2";
    while (*nome2 != '\0')
        std::cout << *(nome2++) << std::endl;

    
    return 0;
}