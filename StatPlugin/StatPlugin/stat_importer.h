#pragma once
#ifndef STAT_IMPORTER_H
#define STAT_IMPORTER_H

#include <fstream>
#include <iostream>
#include <sstream>
#include <vector>
#include <map>

using namespace std;

namespace statimporter
{
    class STAT_IMPORTER {
    private:    
        ifstream inFile;

    public:
        // put these as private later
        map<string, int> stats;
        map<std::string, map<std::string, vector<int>>> attacks_list;

        void read_stats(string fileName); //reads stats from inFile

        vector<string> split(string s, string delimiter); //splits string on delimiter

        void create_player();   //sends stat information to new player/enemy object
    
    };
}

#endif
