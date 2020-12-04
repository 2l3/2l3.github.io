<?php

namespace App\Console\Commands;

use App\Models\Language;
use Illuminate\Console\Command;

class ExportTranslations extends Command
{
    /**
     * The name and signature of the console command.
     *
     * @var string
     */
    protected $signature = 'tools:translations';

    /**
     * The console command description.
     *
     * @var string
     */
    protected $description = 'Update translations from the source code';

    /**
     * Create a new command instance.
     *
     * @return void
     */
    public function __construct()
    {
        parent::__construct();
    }

    /**
     * Execute the console command.
     *
     * @return mixed
     */
    public function handle()
    {
        $languages = Language::all();
        foreach ($languages as $index => $language) {
            $this->info('Step '.++$index.' of '.count($languages).' updating language '.$language->name);
            $this->call('translatable:export', ['lang' => $language->locale]);
        }
        return true;
    }
}
