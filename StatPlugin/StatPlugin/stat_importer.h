#define STAT_IMPORTER_H __declspec(dllexport)
#include <fstream>
#include <iostream>
#include <sstream>
#include <string>
#include <vector>
#include <list>
#include <map>

using namespace std;

extern "C" {
    STAT_IMPORTER_H void read_stats(std::list<std::string> statKeys, std::list<int> statValues, std::list<std::string> attackKeys, 
                                       std::list<int> attackDmg, std::list<int> attackAccuracy, std::string fileName); //reads stats from inFile
    STAT_IMPORTER_H vector<std::string> split(std::string s, std::string delimiter); //splits string on delimiter    
}
