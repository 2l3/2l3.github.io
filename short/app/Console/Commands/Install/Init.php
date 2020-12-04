<?php

namespace App\Console\Commands\Install;

use Illuminate\Console\Command;

class Init extends Command
{
    /**
     * The name and signature of the console command.
     *
     * @var string
     */
    protected $signature = 'install:init';

    /**
     * The console command description.
     *
     * @var string
     */
    protected $description = 'Execute the actions to perform the system installation';

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
        $this->call('down');
        // Database tables
        $this->info('Step 1 of 5 - Creating the database tables');
        $this->call('migrate');
        // Database seeding
        $this->info('Step 2 of 5 - Introducing the data in the database');
        $this->call('db:seed');
        // Generate key
        $this->info('Step 3 of 5 - Generating the unique key of the application');
        $this->call('key:generate');
        // Generate jwt key
        $this->info('Step 4 of 5 - Generating the authentication key');
        $this->call('jwt:secret');
        // Make storage link
        $this->info('Step 5 of 5 - Making storage link');
        $this->call('storage:link');
        // Success
        file_put_contents(storage_path('framework/cache/installed'), date('Y/m/d h:i:s').PHP_EOL, FILE_APPEND | LOCK_EX);
        $this->call('up');
        $this->info('Installation completed successfully');
        return true;
    }
}
