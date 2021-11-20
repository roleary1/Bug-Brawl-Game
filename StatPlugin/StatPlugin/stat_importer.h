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
    STAT_IMPORTER_H void read_stats(list<const char*> statKeys, list<int> statValues, list<const char*> attackKeys,
                                    list<int> attackDmg, list<int> attackAccuracy, const char* fileName);  //reads stats from inFile
    STAT_IMPORTER_H vector<std::string> split(std::string s, std::string delimiter); //splits string on delimiter    
}
