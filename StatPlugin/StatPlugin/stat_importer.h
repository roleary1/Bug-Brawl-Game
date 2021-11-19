#define STAT_IMPORTER_H __declspec(dllexport)
#include <fstream>
#include <iostream>
#include <sstream>
#include <string>
#include <vector>
#include <map>

using namespace std;

extern "C" {
    STAT_IMPORTER_H void read_stats(map<string, int> stats, 
        map<std::string, map<std::string, vector<int>>> attacks_list, std::string fileName); //reads stats from inFile
    STAT_IMPORTER_H vector<std::string> split(std::string s, std::string delimiter); //splits string on delimiter    
}
