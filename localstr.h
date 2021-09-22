#pragma once

enum language : unsigned int // taken from TR7.pdb
{
	language_english = 0,
	language_french = 1,
	language_german = 2,
	language_italian = 3,
	language_spanish = 4,
	language_japanese = 5,
	language_portugese = 6,
	language_polish = 7,
	language_ukenglish = 8,
	language_russian = 9,
	language_czech = 10
};

struct LocalizationHeader
{
	language language;
	int numStrings;
};

constexpr auto ERROR_PREFIX = "\033[91merror:\033[0m ";
