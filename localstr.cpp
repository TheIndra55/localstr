#include <string>
#include <fstream>
#include <iostream>
#include "localstr.h"

int main(int argc, char* argv[])
{
	if (argc == 1)
	{
		std::cout << ERROR_PREFIX << "Usage: localstr.exe [locals.bin]" << std::endl;
		return 1;
	}

	std::ifstream file(argv[1], std::ios::binary);

	if (!file.good())
	{
		std::cout << ERROR_PREFIX << "Failed to open locale file: " << strerror(errno) << std::endl;
		return 1;
	}

	LocalizationHeader header;
	file.read((char*)&header, sizeof(LocalizationHeader));

	std::cout << "File has " << header.numStrings << " strings" << std::endl;
	std::cout << "Language is " << header.language << std::endl;

	for (int i = 0; i < header.numStrings; i++)
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

		std::cout << i << "		" << str << std::endl;

		// return to old cursor
		file.seekg(cursor);
	}
}
