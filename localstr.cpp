#include <string>
#include <fstream>
#include <iostream>
#include "localstr.h"

int main(int argc, char* argv[])
{
	if (argc == 1)
	{
		std::cout << "Usage: localstr.exe [locals.bin]" << std::endl;
		return 1;
	}

	std::ifstream file(argv[1], std::ios::binary);

	if (!file.good())
	{
		std::cout << "Failed to open locale file " << strerror(errno) << std::endl;
		return 1;
	}

	// read the lang
	language lang;
	file.read((char*)&lang, sizeof(int));

	// read the len
	int len;
	file.read((char*)&len, sizeof(int));

	int numStrings = len / 4;
	std::cout << "File has " << numStrings << " strings" << std::endl;
	std::cout << "Language is " << lang << std::endl;

	// seek to offset 12 and start reading
	file.seekg(12);
	for (int i = 0; i < numStrings; i++)
	{
		// read the offset of the string
		int offset;
		file.read((char*)&offset, sizeof(int));

		// store the current position and move to string
		auto cursor = file.tellg();
		file.seekg(offset);

		// read the string
		std::string str;
		std::getline(file, str, '\0');

		std::cout << i << " " << str << std::endl;

		// return to old cursor
		file.seekg(cursor);
	}
}