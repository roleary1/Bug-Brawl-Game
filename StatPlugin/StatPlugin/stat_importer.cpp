#include "stat_importer.h"

using namespace statimporter;

void STAT_IMPORTER::import_file(String fileName) {
	inFile.open(fileName);

	if (!inFile) {
		cerr << "Unable to open file datafile.txt";
		return;
	}

	while (inFile >> x) {
		//read in stats
	}


	inFile.close()
}

void STAT_IMPORTER::update_stats() {
	
}

void STAT_IMPORTER::create_player() {

}