#pragma once
#ifndef STAT_IMPORTER_H
#define STAT_IMPORTER_H

#include <fstream>

using namespace std;

namespace statimporter
{
    class STAT_IMPORTER {
    private:    
        ifstream inFile;
        String[] stats;

    public:
        void import_file(String fileName); //sets the inFile to read

        void update_stats();    //parses inFile and reads stat fields

        void create_player();   //sends stat information to new player/enemy object
    
    };
}

#endif
